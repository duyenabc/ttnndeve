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
        var origins = builder.Configuration["Cors:Origins"]
            ?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (origins is { Length: > 0 })
        {
            policy.WithOrigins(origins)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        }
        else if (builder.Environment.IsDevelopment())
        {
            policy.WithOrigins(
                    "http://localhost:3000",
                    "http://localhost:5173",
                    "http://127.0.0.1:3000",
                    "http://127.0.0.1:5173")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        }
        else
        {
            // Temporary until Cors:Origins is set on Render
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
    return Results.Ok(new { status = "ok", users, database = "ready" });
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
