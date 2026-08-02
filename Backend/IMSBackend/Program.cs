using System.Text;
using IMSBackend.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Render (and most PaaS) inject PORT
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrWhiteSpace(port))
{
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var hasDatabaseUrlEnv = !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("DATABASE_URL"));
var hasCsEnv = !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection"));
Console.WriteLine($"[IMS] Env DATABASE_URL set: {hasDatabaseUrlEnv}; ConnectionStrings__DefaultConnection set: {hasCsEnv}");

var connectionString = ResolveConnectionString(builder.Configuration);
Console.WriteLine($"[IMS] DB host hint: {DescribeConnectionTarget(connectionString)}");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString, npgsql =>
        npgsql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(5), null)));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp", policy =>
    {
        var configured = builder.Configuration["Cors:Origins"]
            ?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            ?? Array.Empty<string>();

        // Always allow local Vite + configured production origins (e.g. Render static site)
        var localDev = new[]
        {
            "http://localhost:3000",
            "http://localhost:5173",
            "http://127.0.0.1:3000",
            "http://127.0.0.1:5173",
        };
        var origins = configured.Concat(localDev).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();

        if (origins.Length > 0)
        {
            policy.WithOrigins(origins)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        }
        else
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
    });
});

var jwtKey = builder.Configuration["Jwt:Key"]
    ?? Environment.GetEnvironmentVariable("Jwt__Key")
    ?? "SuperSecretKey_For_Development_IMS_12345";
var key = Encoding.UTF8.GetBytes(jwtKey);
if (key.Length < 32)
{
    // HMAC-SHA256 needs a sufficiently long key
    key = Encoding.UTF8.GetBytes(jwtKey.PadRight(32, '0'));
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

var app = builder.Build();
var dbReady = false;
string? dbError = null;

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowVueApp");

app.Use(async (ctx, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[IMS] Unhandled: {ex.GetType().Name}: {ex.Message}");
        if (ex.InnerException != null)
            Console.WriteLine($"[IMS] Inner: {ex.InnerException.Message}");
        if (ctx.Response.HasStarted) throw;
        ctx.Response.StatusCode = 500;
        ctx.Response.ContentType = "application/json";
        await ctx.Response.WriteAsJsonAsync(new
        {
            message = "Lỗi máy chủ khi xử lý yêu cầu.",
            detail = app.Environment.IsDevelopment() ? ex.Message : ex.GetType().Name
        });
    }
});

// TLS is terminated by Render — skip HTTPS redirect in containers
if (!app.Environment.IsDevelopment() && string.IsNullOrWhiteSpace(port))
{
    app.UseHttpsRedirection();
}

app.Use(async (ctx, next) =>
{
    var path = ctx.Request.Path.Value ?? "";
    var needsDb = path.StartsWith("/api", StringComparison.OrdinalIgnoreCase);
    if (needsDb && !dbReady)
    {
        ctx.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
        ctx.Response.ContentType = "application/json";
        await ctx.Response.WriteAsJsonAsync(new
        {
            message = "Database đang khởi tạo hoặc chưa kết nối được. Thử lại sau vài giây.",
            detail = dbError
        });
        return;
    }
    await next();
});

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => Results.Ok(new
{
    service = "IMS API",
    status = "ok",
    docs = "/openapi/v1.json",
    database = dbReady ? "ready" : "starting"
}));

app.MapGet("/api/health/db", async (AppDbContext db) =>
{
    var users = await db.Users.CountAsync();
    var classes = await db.Classes.CountAsync();
    var hasGv001 = await db.Users.AnyAsync(u => u.MaDinhDanh != null && u.MaDinhDanh.ToLower() == "gv001");
    var demoSv = await db.Users.CountAsync(u => u.VaiTro == "SinhVien" && u.LopSinhHoat != null && u.LopSinhHoat.StartsWith("LOP10"));
    // #region agent log
    try
    {
        const string logPath = @"C:\Users\while\Downloads\remix_-ttnndev (1)\debug-19ef33.log";
        var line = System.Text.Json.JsonSerializer.Serialize(new
        {
            sessionId = "19ef33",
            hypothesisId = "A,B,C",
            location = "Program.cs:health/db",
            message = "health db snapshot",
            data = new { users, classes, hasGv001, demoSv },
            timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            runId = "demo-restore"
        });
        await File.AppendAllTextAsync(logPath, line + Environment.NewLine);
    }
    catch { /* ignore debug log IO */ }
    // #endregion
    return Results.Ok(new { status = "ok", users, classes, hasGv001, demoSv, database = "ready" });
});

// Listen first so Render healthCheckPath:/ succeeds, then init DB
await app.StartAsync();
Console.WriteLine("[IMS] HTTP server listening; initializing database...");
try
{
    await EnsureDatabaseReadyAsync(app.Services);
    dbReady = true;
    Console.WriteLine("[IMS] Database ready flag set.");
}
catch (Exception ex)
{
    dbError = ex.Message;
    Console.WriteLine($"[IMS] Database init FAILED: {ex}");
    // Keep serving / so health checks work; /api returns 503 until fixed + restart
}
await app.WaitForShutdownAsync();

static async Task EnsureDatabaseReadyAsync(IServiceProvider services)
{
    const int maxAttempts = 10;
    Exception? last = null;

    for (var attempt = 1; attempt <= maxAttempts; attempt++)
    {
        try
        {
            using var scope = services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await db.Database.OpenConnectionAsync();
            await db.Database.CloseConnectionAsync();
            await db.Database.EnsureCreatedAsync();
            // EnsureCreated does not add new tables/columns to an existing DB
            await EnsureDiaryFeedbackSchemaAsync(db);
            await SeedDefaultsIfEmptyAsync(db);
            await EnsureDemoDatasetAsync(db);
            Console.WriteLine($"[IMS] Database ready (attempt {attempt}).");
            return;
        }
        catch (Exception ex)
        {
            last = ex;
            Console.WriteLine($"[IMS] DB not ready (attempt {attempt}/{maxAttempts}): {ex.GetType().Name}: {ex.Message}");
            if (ex.InnerException != null)
                Console.WriteLine($"[IMS] Inner: {ex.InnerException.Message}");
            await Task.Delay(TimeSpan.FromSeconds(Math.Min(30, attempt * 3)));
        }
    }

    throw new InvalidOperationException(
        "Could not connect to PostgreSQL after retries. " +
        "Check DATABASE_URL is the Internal Database URL from ims-db, " +
        "and that the API service is in the same region as ims-db (Oregon).",
        last);
}

static async Task EnsureDiaryFeedbackSchemaAsync(AppDbContext db)
{
    // Patch schema for DBs created before Feedbacks/Notifications existed
    await db.Database.ExecuteSqlRawAsync("""
        CREATE TABLE IF NOT EXISTS "Feedbacks" (
            "Id" text NOT NULL,
            "DiaryId" text NULL,
            "TeacherName" text NULL,
            "Content" text NULL,
            "Timestamp" timestamp with time zone NOT NULL,
            CONSTRAINT "PK_Feedbacks" PRIMARY KEY ("Id"),
            CONSTRAINT "FK_Feedbacks_Diaries_DiaryId" FOREIGN KEY ("DiaryId")
                REFERENCES "Diaries" ("Id") ON DELETE CASCADE
        );
        """);
    await db.Database.ExecuteSqlRawAsync("""
        CREATE TABLE IF NOT EXISTS "Notifications" (
            "Id" text NOT NULL,
            "TieuDe" text NULL,
            "NoiDung" text NULL,
            "Type" text NULL,
            "Role" text NULL,
            "UserId" text NULL,
            "Icon" text NULL,
            "BgClass" text NULL,
            "Link" text NULL,
            "IsRead" boolean NOT NULL DEFAULT FALSE,
            "NgayTao" timestamp with time zone NOT NULL,
            CONSTRAINT "PK_Notifications" PRIMARY KEY ("Id")
        );
        """);
    await db.Database.ExecuteSqlRawAsync("""
        ALTER TABLE "Diaries" ADD COLUMN IF NOT EXISTS "IsReadByTeacher" boolean NOT NULL DEFAULT FALSE;
        ALTER TABLE "Diaries" ADD COLUMN IF NOT EXISTS "IsReadByStudent" boolean NOT NULL DEFAULT TRUE;
        ALTER TABLE "Diaries" ADD COLUMN IF NOT EXISTS "ClassId" text NULL;
        """);
    Console.WriteLine("[IMS] Diary/Feedback/Notification schema ensured.");
}

static async Task SeedDefaultsIfEmptyAsync(AppDbContext db)
{
    if (await db.Users.AnyAsync())
    {
        Console.WriteLine("[IMS] Users already present; skip seed.");
        return;
    }

    var created = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    db.Users.AddRange(
        new IMSBackend.Models.User
        {
            MaNguoiDung = "admin_001",
            MaDinhDanh = "admin",
            HoTen = "Quản trị viên",
            Email = "admin@example.com",
            VaiTro = "Admin",
            TrangThaiTaiKhoan = "DangHoatDong",
            MatKhau = "Admin@123",
            BuocDoiMatKhau = false,
            QuyenQuanLyNguoiDung = true,
            NgayTao = created
        },
        new IMSBackend.Models.User
        {
            MaNguoiDung = "gv_001",
            MaDinhDanh = "GV001",
            HoTen = "ThS. Lê Hoàng Nam",
            Email = "namlh@example.com",
            VaiTro = "GiangVien",
            TrangThaiTaiKhoan = "DangHoatDong",
            MatKhau = "Gv@12345",
            BuocDoiMatKhau = false,
            NgayTao = created
        },
        new IMSBackend.Models.User
        {
            MaNguoiDung = "sv_001",
            MaDinhDanh = "SV001",
            HoTen = "Nguyễn Văn A",
            Email = "sv001@example.com",
            VaiTro = "SinhVien",
            TrangThaiTaiKhoan = "DangHoatDong",
            MatKhau = "Sv@12345",
            BuocDoiMatKhau = false,
            NgayTao = created,
            LopSinhHoat = "K64-CNTT"
        });
    await db.SaveChangesAsync();
    Console.WriteLine("[IMS] Seeded default users (admin, GV001, SV001).");
}

/// <summary>
/// Restores the historical demo set (Firestore-era): gv001 + Test@1234, LOP101–105, MSSV lists.
/// Upserts — safe to run on every startup; does not delete other users.
/// </summary>
static async Task EnsureDemoDatasetAsync(AppDbContext db)
{
    const string demoPassword = "Test@1234";
    var created = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    var addedUsers = 0;
    var addedClasses = 0;

    async Task<IMSBackend.Models.User> UpsertUserAsync(
        string maNguoiDung,
        string maDinhDanh,
        string hoTen,
        string vaiTro,
        string? email = null,
        string? lopSinhHoat = null)
    {
        var user = await db.Users.FirstOrDefaultAsync(u =>
            u.MaNguoiDung == maNguoiDung ||
            (u.MaDinhDanh != null && u.MaDinhDanh.ToLower() == maDinhDanh.ToLower()));

        if (user == null)
        {
            user = new IMSBackend.Models.User
            {
                MaNguoiDung = maNguoiDung,
                MaDinhDanh = maDinhDanh,
                HoTen = hoTen,
                Email = email ?? $"{maDinhDanh.ToLower()}@due.udn.vn",
                VaiTro = vaiTro,
                TrangThaiTaiKhoan = "DangHoatDong",
                MatKhau = demoPassword,
                BuocDoiMatKhau = false,
                NgayTao = created,
                LopSinhHoat = lopSinhHoat
            };
            db.Users.Add(user);
            addedUsers++;
        }
        else
        {
            // Keep login usable for the known demo password set
            user.MaDinhDanh = maDinhDanh;
            user.HoTen = hoTen;
            user.VaiTro = vaiTro;
            user.TrangThaiTaiKhoan = "DangHoatDong";
            user.MatKhau = demoPassword;
            user.BuocDoiMatKhau = false;
            if (!string.IsNullOrWhiteSpace(lopSinhHoat))
                user.LopSinhHoat = lopSinhHoat;
            if (string.IsNullOrWhiteSpace(user.Email))
                user.Email = email ?? $"{maDinhDanh.ToLower()}@due.udn.vn";
        }

        return user;
    }

    var gv = await UpsertUserAsync("gv_demo_001", "gv001", "ThS. Demo Hướng Dẫn", "GiangVien", "gv001@due.udn.vn");

    // Class → student MSSV lists (as provided by product owner)
    var classStudents = new Dictionary<string, string[]>
    {
        ["LOP101"] = new[]
        {
            "231121521101", "20241021", "20246015", "20246012",
            "20246020", "20246022", "20246025", "20246028"
        },
        ["LOP102"] = new[] { "20246030", "20246031", "20246032", "20246033", "20246034", "20246035" },
        ["LOP103"] = new[] { "20246036", "20246037", "20246038", "20246039", "20246040", "20246041" },
        ["LOP104"] = new[] { "20246050", "20246051", "20246052" },
        ["LOP105"] = new[] { "20246053", "20246054", "20246055" },
    };

    foreach (var (maLop, mssvs) in classStudents)
    {
        var cls = await db.Classes.FirstOrDefaultAsync(c => c.Id == maLop || c.MaLop == maLop);
        if (cls == null)
        {
            cls = new IMSBackend.Models.Class
            {
                Id = maLop,
                MaLop = maLop,
                TenLop = $"Lớp thực tập {maLop}",
                GiangVienId = gv.MaNguoiDung,
                SoSinhVien = mssvs.Length
            };
            db.Classes.Add(cls);
            addedClasses++;
        }
        else
        {
            cls.MaLop = maLop;
            cls.TenLop = string.IsNullOrWhiteSpace(cls.TenLop) ? $"Lớp thực tập {maLop}" : cls.TenLop;
            cls.GiangVienId = gv.MaNguoiDung;
            cls.SoSinhVien = mssvs.Length;
        }

        var i = 1;
        foreach (var mssv in mssvs)
        {
            await UpsertUserAsync(
                maNguoiDung: $"sv_{mssv}",
                maDinhDanh: mssv,
                hoTen: $"Sinh viên {mssv}",
                vaiTro: "SinhVien",
                email: $"{mssv}@student.due.udn.vn",
                lopSinhHoat: maLop);
            i++;
        }
    }

    await db.SaveChangesAsync();
    var totalUsers = await db.Users.CountAsync();
    var totalClasses = await db.Classes.CountAsync();
    var hasGv001 = await db.Users.AnyAsync(u => u.MaDinhDanh != null && u.MaDinhDanh.ToLower() == "gv001");
    var demoSv = await db.Users.CountAsync(u => u.VaiTro == "SinhVien" && u.LopSinhHoat != null && u.LopSinhHoat.StartsWith("LOP10"));
    Console.WriteLine(
        $"[IMS] Demo dataset ensured (+users={addedUsers}, +classes={addedClasses}); totals users={totalUsers}, classes={totalClasses}.");
    // #region agent log
    try
    {
        const string logPath = @"C:\Users\while\Downloads\remix_-ttnndev (1)\debug-19ef33.log";
        var line = System.Text.Json.JsonSerializer.Serialize(new
        {
            sessionId = "19ef33",
            hypothesisId = "B,D",
            location = "Program.cs:EnsureDemoDatasetAsync",
            message = "demo dataset upsert finished",
            data = new { addedUsers, addedClasses, totalUsers, totalClasses, hasGv001, demoSv },
            timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            runId = "demo-restore"
        });
        await File.AppendAllTextAsync(logPath, line + Environment.NewLine);
    }
    catch { /* ignore debug log IO */ }
    // #endregion
}

static string DescribeConnectionTarget(string cs)
{
    try
    {
        var host = cs.Split(';')
            .Select(p => p.Trim())
            .FirstOrDefault(p => p.StartsWith("Host=", StringComparison.OrdinalIgnoreCase));
        var db = cs.Split(';')
            .Select(p => p.Trim())
            .FirstOrDefault(p => p.StartsWith("Database=", StringComparison.OrdinalIgnoreCase));
        return $"{host ?? "Host=?"};{db ?? "Database=?"}";
    }
    catch
    {
        return "(unparsed)";
    }
}

static string ResolveConnectionString(IConfiguration config)
{
    // Prefer env (Render), then appsettings — never use localhost on Render/Production
    var fromEnv = Environment.GetEnvironmentVariable("DATABASE_URL")
        ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
    var databaseUrl = fromEnv ?? config.GetConnectionString("DefaultConnection");
    var onRenderOrProd =
        !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("PORT"))
        || string.Equals(
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
            "Production",
            StringComparison.OrdinalIgnoreCase);

    if (string.IsNullOrWhiteSpace(databaseUrl))
    {
        if (onRenderOrProd)
        {
            throw new InvalidOperationException(
                "DATABASE_URL is missing. In Render → ims-api-ftzr → Environment: " +
                "add DATABASE_URL = Internal Database URL from your Postgres (ims-db), then redeploy.");
        }

        return "Host=localhost;Port=5432;Database=ims_db;Username=postgres;Password=postgres";
    }

    string resolved;
    if (databaseUrl.Contains("Host=", StringComparison.OrdinalIgnoreCase)
        || databaseUrl.Contains("Server=", StringComparison.OrdinalIgnoreCase))
    {
        resolved = EnsureSsl(databaseUrl);
    }
    else if (databaseUrl.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase)
        || databaseUrl.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase))
    {
        var uri = new Uri(databaseUrl);
        var userInfo = uri.UserInfo.Split(':', 2);
        var username = Uri.UnescapeDataString(userInfo[0]);
        var password = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : "";
        var database = uri.AbsolutePath.Trim('/');
        var dbPort = uri.Port > 0 ? uri.Port : 5432;

        resolved = EnsureSsl(
            $"Host={uri.Host};Port={dbPort};Database={database};Username={username};Password={password}");
    }
    else
    {
        throw new InvalidOperationException(
            "DATABASE_URL must be a postgresql:// URL or Host=... Npgsql connection string.");
    }

    if (onRenderOrProd && IsLocalDbHost(resolved))
    {
        throw new InvalidOperationException(
            "Database host is localhost inside the container. " +
            "Set DATABASE_URL to the Render Postgres Internal Database URL " +
            "(host looks like dpg-xxxxx-a), not localhost. Then Manual Deploy the API.");
    }

    Console.WriteLine($"[IMS] Connection source: {(fromEnv != null ? "environment" : "appsettings")}");
    return resolved;
}

static bool IsLocalDbHost(string connectionString)
{
    var host = connectionString.Split(';')
        .Select(p => p.Trim())
        .FirstOrDefault(p => p.StartsWith("Host=", StringComparison.OrdinalIgnoreCase)
            || p.StartsWith("Server=", StringComparison.OrdinalIgnoreCase));
    if (host == null) return false;
    var value = host.Split('=', 2)[1].Trim();
    return value is "localhost" or "127.0.0.1" or "::1";
}

static string EnsureSsl(string connectionString)
{
    // Render Postgres: external needs TLS; internal accepts Prefer.
    // Trust Server Certificate avoids CA issues in containers.
    if (!connectionString.Contains("SSL Mode=", StringComparison.OrdinalIgnoreCase)
        && !connectionString.Contains("Ssl Mode=", StringComparison.OrdinalIgnoreCase))
    {
        connectionString += ";SSL Mode=Prefer";
    }

    if (!connectionString.Contains("Trust Server Certificate=", StringComparison.OrdinalIgnoreCase))
    {
        connectionString += ";Trust Server Certificate=true";
    }

    if (!connectionString.Contains("Timeout=", StringComparison.OrdinalIgnoreCase))
    {
        connectionString += ";Timeout=30";
    }

    return connectionString;
}
