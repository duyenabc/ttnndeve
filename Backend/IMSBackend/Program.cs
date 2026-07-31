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

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowVueApp");

// TLS is terminated by Render — skip HTTPS redirect in containers
if (!app.Environment.IsDevelopment() && string.IsNullOrWhiteSpace(port))
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => Results.Ok(new
{
    service = "IMS API",
    status = "ok",
    docs = "/openapi/v1.json"
}));

// Listen first so Render healthCheckPath:/ succeeds, then init DB
await app.StartAsync();
Console.WriteLine("[IMS] HTTP server listening; initializing database...");
await EnsureDatabaseReadyAsync(app.Services);
await app.WaitForShutdownAsync();

static async Task EnsureDatabaseReadyAsync(IServiceProvider services)
{
    const int maxAttempts = 10;
    Exception last = null;

    for (var attempt = 1; attempt <= maxAttempts; attempt++)
    {
        try
        {
            using var scope = services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await db.Database.OpenConnectionAsync();
            await db.Database.CloseConnectionAsync();
            await db.Database.EnsureCreatedAsync();
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
        "and that ims-api is in the same region as ims-db (Oregon).",
        last);
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
    // Prefer DATABASE_URL (Render), then ConnectionStrings__DefaultConnection
    var databaseUrl =
        Environment.GetEnvironmentVariable("DATABASE_URL")
        ?? config.GetConnectionString("DefaultConnection");

    if (string.IsNullOrWhiteSpace(databaseUrl))
    {
        return "Host=localhost;Port=5432;Database=ims_db;Username=postgres;Password=postgres";
    }

    // Already an Npgsql key=value string
    if (databaseUrl.Contains("Host=", StringComparison.OrdinalIgnoreCase)
        || databaseUrl.Contains("Server=", StringComparison.OrdinalIgnoreCase))
    {
        return EnsureSsl(databaseUrl);
    }

    if (databaseUrl.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase)
        || databaseUrl.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase))
    {
        var uri = new Uri(databaseUrl);
        var userInfo = uri.UserInfo.Split(':', 2);
        var username = Uri.UnescapeDataString(userInfo[0]);
        var password = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : "";
        var database = uri.AbsolutePath.Trim('/');
        var port = uri.Port > 0 ? uri.Port : 5432;

        return EnsureSsl(
            $"Host={uri.Host};Port={port};Database={database};Username={username};Password={password}");
    }

    throw new InvalidOperationException(
        "DATABASE_URL must be a postgresql:// URL or Host=... Npgsql connection string.");
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
