using System.IdentityModel.Tokens.Jwt;
using Microsoft.Data.SqlClient;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using MiniCrm.Api.BackgroundServices;
using MiniCrm.Api.ErrorHandling;
using MiniCrm.Api.Hubs;
using MiniCrm.Api.Middleware;
using MiniCrm.Api.OpenApi;
using MiniCrm.Api.Realtime;
using MiniCrm.Application.Interfaces.Services;
using MiniCrm.Infrastructure.Authentication;
using MiniCrm.Infrastructure.Email;
using MiniCrm.Infrastructure.Orders;
using MiniCrm.Infrastructure.Security;
using MiniCrm.Persistence;
using MiniCrm.Persistence.Seed;


// ================================================================
// BUILDER
// ================================================================

var builder =
    WebApplication.CreateBuilder(
        args);


// ================================================================
// DATABASE
// ================================================================

var connectionString =
    builder.Configuration
        .GetConnectionString(
            "DefaultConnection")
    ?? throw new InvalidOperationException(
        "DefaultConnection was not found.");


builder.Services.AddPersistence(
    connectionString);


// ================================================================
// LOCALDB KEEP-ALIVE
//
// LocalDB boşta kaldığında kendini kapatıp bir sonraki bağlantıda
// arada sırada başlatma hatası veriyor. Bu servis düzenli aralıklarla
// hafif bir sorgu göndererek bunu önler.
// ================================================================

builder.Services.AddHostedService<LocalDbKeepAliveService>();


// ================================================================
// JWT
// ================================================================

var jwtSection =
    builder.Configuration.GetSection(
        "Jwt");


var jwtSettings =
    new JwtSettings
    {
        Issuer =
            jwtSection["Issuer"]
            ?? throw new InvalidOperationException(
                "Jwt:Issuer was not found."),

        Audience =
            jwtSection["Audience"]
            ?? throw new InvalidOperationException(
                "Jwt:Audience was not found."),

        Key =
            jwtSection["Key"]
            ?? throw new InvalidOperationException(
                "Jwt:Key was not found."),

        ExpirationMinutes =
            int.TryParse(
                jwtSection["ExpirationMinutes"],
                out var expirationMinutes)
                ? expirationMinutes
                : 60
    };


builder.Services.AddSingleton(
    jwtSettings);


builder.Services.AddScoped<
    ITokenService,
    JwtTokenService>();


// ================================================================
// VERIFICATION CODE
// ================================================================

var verificationSettings =
    new VerificationCodeSettings
    {
        HashKey =
            builder.Configuration[
                "Verification:HashKey"]
            ?? throw new InvalidOperationException(
                "Verification:HashKey was not found.")
    };


builder.Services.AddSingleton(
    verificationSettings);


builder.Services.AddSingleton<
    IVerificationCodeService,
    VerificationCodeService>();


builder.Services.AddSingleton<
    IOrderNumberGenerator,
    OrderNumberGenerator>();


// ================================================================
// SMTP
// ================================================================

var smtpSection =
    builder.Configuration.GetSection(
        "Smtp");


var smtpSettings =
    new SmtpSettings
    {
        Host =
            smtpSection["Host"]
            ?? throw new InvalidOperationException(
                "Smtp:Host was not found."),

        Port =
            int.TryParse(
                smtpSection["Port"],
                out var smtpPort)
                ? smtpPort
                : 587,

        EnableSsl =
            !bool.TryParse(
                smtpSection["EnableSsl"],
                out var enableSsl)
            || enableSsl,

        Username =
            smtpSection["Username"]
            ?? throw new InvalidOperationException(
                "Smtp:Username was not found."),

        Password =
            smtpSection["Password"]
            ?? throw new InvalidOperationException(
                "Smtp:Password was not found."),

        FromEmail =
            smtpSection["FromEmail"]
            ?? throw new InvalidOperationException(
                "Smtp:FromEmail was not found."),

        FromName =
            smtpSection["FromName"]
            ?? "MiniCrm"
    };


builder.Services.AddSingleton(
    smtpSettings);


builder.Services.AddScoped<
    IEmailService,
    SmtpEmailService>();


// ================================================================
// AUTHENTICATION
// ================================================================

builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(
        options =>
        {
            options.MapInboundClaims =
                false;


            options.TokenValidationParameters =
                new TokenValidationParameters
                {
                    ValidateIssuer =
                        true,

                    ValidIssuer =
                        jwtSettings.Issuer,

                    ValidateAudience =
                        true,

                    ValidAudience =
                        jwtSettings.Audience,

                    ValidateLifetime =
                        true,

                    ValidateIssuerSigningKey =
                        true,

                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(
                                jwtSettings.Key)),

                    ClockSkew =
                        TimeSpan.Zero,

                    NameClaimType =
                        JwtRegisteredClaimNames.Email,

                    RoleClaimType =
                        ClaimTypes.Role
                };


            // SignalR WebSocket/SSE bağlantılarında Authorization header
            // her zaman kullanılamadığından, client JWT'yi query string
            // üzerinden (access_token) gönderir; bu standart yaklaşım
            // yalnızca hub path'i için token'ı buradan okur.
            options.Events =
                new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken =
                            context.Request.Query["access_token"];

                        var path =
                            context.HttpContext.Request.Path;

                        if (!string.IsNullOrEmpty(accessToken) &&
                            path.StartsWithSegments("/hubs"))
                        {
                            context.Token =
                                accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
        });


builder.Services.AddAuthorization();


// ================================================================
// SIGNALR
// ================================================================

builder.Services.AddSignalR();

builder.Services.AddScoped<
    IRealtimeNotifier,
    SignalRNotifier>();


// ================================================================
// PROBLEM DETAILS + GLOBAL EXCEPTION HANDLER
// ================================================================

builder.Services.AddProblemDetails(
    options =>
    {
        options.CustomizeProblemDetails =
            context =>
            {
                context.ProblemDetails
                    .Extensions["traceId"] =
                    context.HttpContext
                        .TraceIdentifier;
            };
    });


builder.Services.AddExceptionHandler<
    GlobalExceptionHandler>();


// ================================================================
// RATE LIMITING
//
// /api/Auth is limited per remote IP.
// Other API endpoints are not globally throttled here.
// ================================================================

builder.Services.AddRateLimiter(
    options =>
    {
        options.RejectionStatusCode =
            StatusCodes
                .Status429TooManyRequests;


        options.GlobalLimiter =
            PartitionedRateLimiter
                .Create<HttpContext, string>(
                    httpContext =>
                    {
                        var path =
                            httpContext
                                .Request
                                .Path
                                .Value
                            ?? string.Empty;


                        if (path.StartsWith(
                                "/api/Auth",
                                StringComparison
                                    .OrdinalIgnoreCase))
                        {
                            var ipAddress =
                                httpContext
                                    .Connection
                                    .RemoteIpAddress?
                                    .ToString()
                                ?? "unknown";


                            return RateLimitPartition
                                .GetFixedWindowLimiter(
                                    partitionKey:
                                        $"auth:{ipAddress}",

                                    factory:
                                        _ =>
                                            new FixedWindowRateLimiterOptions
                                            {
                                                AutoReplenishment =
                                                    true,

                                                PermitLimit =
                                                    10,

                                                Window =
                                                    TimeSpan
                                                        .FromMinutes(
                                                            1),

                                                QueueLimit =
                                                    0,

                                                QueueProcessingOrder =
                                                    QueueProcessingOrder
                                                        .OldestFirst
                                            });
                        }


                        return RateLimitPartition
                            .GetNoLimiter(
                                "non-auth");
                    });


        options.OnRejected =
            async (
                context,
                cancellationToken) =>
            {
                var problemDetails =
                    new ProblemDetails
                    {
                        Status =
                            StatusCodes
                                .Status429TooManyRequests,

                        Title =
                            "Too Many Requests",

                        Detail =
                            "Too many authentication requests. Please try again later.",

                        Instance =
                            context.HttpContext
                                .Request
                                .Path
                    };


                problemDetails
                    .Extensions["traceId"] =
                    context.HttpContext
                        .TraceIdentifier;


                context.HttpContext
                    .Response
                    .ContentType =
                    "application/problem+json";


                await context
                    .HttpContext
                    .Response
                    .WriteAsJsonAsync(
                        problemDetails,
                        cancellationToken);
            };
    });


// ================================================================
// HEALTH CHECK
// ================================================================

builder.Services.AddHealthChecks();


// ================================================================
// MVC / OPENAPI
// ================================================================

builder.Services.AddControllers();


builder.Services.AddOpenApi(
    options =>
    {
        options.AddDocumentTransformer<
            BearerSecuritySchemeTransformer>();
    });


// ================================================================
// BUILD APP
// ================================================================

var app =
    builder.Build();


// ================================================================
// VERİTABANINI ISIT
//
// Burada iki yaklaşım denendi ve ikisi de tek başına yetmedi:
//   1) EF Core bağlanmadan önce elle "sqllocaldb start" çalıştırmak —
//      bu, ADO.NET'in kendi pasif "Auto-create" başlatmasıyla AYNI ANDA
//      yarışa girip "WaitForMultipleObjects" hatasına (error 575) yol
//      açabiliyordu.
//   2) Sadece pasif auto-create'e güvenip hiç elle müdahale etmemek —
//      bu da bazı ortamlarda (özellikle Visual Studio'nun API+WinFormUI
//      projelerini birlikte debug modunda başlatıp sistemi
//      yoğunlaştırdığı anlarda) LocalDB'nin kendi kendine hiç
//      toparlanamadığı, tüm denemelerin tükendiği durumlar yaratıyordu.
//
// Bu yüzden burada üçüncü, TEPKİSEL bir yaklaşım kullanılıyor: ilk
// deneme her zaman saf pasif bağlantıdır (hiçbir şeyle yarışmaz). Bir
// deneme BAŞARISIZ OLDUKTAN SONRA (yalnızca o zaman, önceden değil)
// "sqllocaldb start" elle çalıştırılıp örnek zorla toparlanmaya
// çalışılır, ardından tekrar denenir. Bu, iki mekanizmanın aynı anda
// yarışmasını engellerken (çünkü ikinci mekanizma yalnızca birincisi
// zaten gözlemlenebilir şekilde başarısız olduktan sonra devreye
// giriyor), kullanıcının elle yaptığı "stop/start" düzeltmesinin aynısını
// otomatik olarak, her denemede uygular.
// ================================================================

await WarmUpDatabaseAsync();

async Task WarmUpDatabaseAsync()
{
    var connectionString =
        app.Configuration.GetConnectionString("DefaultConnection");

    if (string.IsNullOrWhiteSpace(connectionString))
    {
        return;
    }

    var instanceName =
        ExtractLocalDbInstanceName(connectionString);

    const int maxWarmUpAttempts = 30;

    for (var attempt = 1; attempt <= maxWarmUpAttempts; attempt++)
    {
        try
        {
            await using var connection =
                new SqlConnection(connectionString);

            await connection.OpenAsync();

            await using var command =
                connection.CreateCommand();

            command.CommandText = "SELECT 1";

            await command.ExecuteScalarAsync();

            return;
        }
        catch when (attempt < maxWarmUpAttempts)
        {
            if (instanceName is not null)
            {
                await TryReactiveLocalDbStartAsync(
                    instanceName);
            }

            await Task.Delay(2000);
        }
        catch
        {
            // Isıtma denemeleri tükendi; asıl bağlantı denemesi
            // aşağıdaki IdentitySeeder döngüsünde yine de yapılacak
            // ve gerçek hata orada, olduğu gibi raporlanacak.
        }
    }
}

static string? ExtractLocalDbInstanceName(
    string connectionString)
{
    const string marker = "(localdb)\\";

    var markerIndex =
        connectionString.IndexOf(
            marker,
            StringComparison.OrdinalIgnoreCase);

    if (markerIndex < 0)
    {
        return null;
    }

    var nameStart =
        markerIndex + marker.Length;

    var nameEnd =
        connectionString.IndexOf(
            ';',
            nameStart);

    return nameEnd < 0
        ? connectionString[nameStart..]
        : connectionString[nameStart..nameEnd];
}

static async Task TryReactiveLocalDbStartAsync(
    string instanceName)
{
    try
    {
        using var process = System.Diagnostics.Process.Start(
            new System.Diagnostics.ProcessStartInfo
            {
                FileName = "sqllocaldb",
                Arguments = $"start {instanceName}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            });

        if (process is not null)
        {
            await process.WaitForExitAsync();
        }
    }
    catch
    {
        // Elle başlatma denemesi başarısız olursa asıl bağlantı
        // denemesi yine de bir sonraki turda tekrar yapılacak.
    }
}


// ================================================================
// IDENTITY SEED
//
// LocalDB bazen uygulama başlarken henüz tam hazır olmuyor (geçici
// "SQL Server process failed to start" hatası) — özellikle Visual
// Studio'nun aynı anda birden fazla projeyi (API + WinFormUI) debug
// modunda başlattığı, sistemin JIT/sembol yükleme yüzünden meşgul
// olduğu senaryolarda WarmUpDatabaseAsync'in ayırdığı süre yetmeyebilir.
// Bu yüzden burada da uzunca bir yeniden deneme penceresi bırakılır.
// Tüm denemeler tükenirse hata olduğu gibi fırlatılmak yerine temiz
// bir mesajla süreç kontrollü şekilde sonlandırılır; aksi halde bu,
// Visual Studio'da "Kullanıcı Tarafından İşlenmeyen Özel Durum" olarak
// görünüp hata ayıklayıcıyı koda düşürüyordu.
// ================================================================

const int maxSeedAttempts = 30;

for (var attempt = 1; attempt <= maxSeedAttempts; attempt++)
{
    try
    {
        await IdentitySeeder.SeedAsync(
            app.Services,
            app.Configuration);

        break;
    }
    catch (Exception exception) when (attempt < maxSeedAttempts)
    {
        Console.WriteLine(
            $"Veritabanına bağlanılamadı (deneme {attempt}/{maxSeedAttempts}), 2 saniye sonra tekrar denenecek: {exception.Message}");

        await Task.Delay(
            TimeSpan.FromSeconds(2));
    }
    catch (Exception exception)
    {
        Console.WriteLine(
            $"""

            ================================================================
            Veritabanına {maxSeedAttempts} denemeden sonra bağlanılamadı.
            LocalDB örneği başlatılamıyor olabilir. Lütfen şunları deneyin:
              1. Uygulamayı kapatıp tekrar başlatın.
              2. Sorun sürerse: "sqllocaldb stop MSSQLLocalDB" ve ardından
                 "sqllocaldb start MSSQLLocalDB" komutlarını çalıştırın.
            Hata: {exception.Message}
            ================================================================
            """);

        Environment.Exit(1);
    }
}


// ================================================================
// DEVELOPMENT
// ================================================================

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();


    app.UseSwaggerUI(
        options =>
        {
            options.SwaggerEndpoint(
                "/openapi/v1.json",
                "MiniCrm API v1");
        });
}


// ================================================================
// PRODUCTION SECURITY
// ================================================================

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();

    app.UseHttpsRedirection();
}


// ================================================================
// SECURITY HEADERS
// ================================================================

app.UseMiddleware<
    SecurityHeadersMiddleware>();


// ================================================================
// EXCEPTION HANDLER
// ================================================================

app.UseExceptionHandler();


// ================================================================
// RATE LIMITER
// ================================================================

app.UseRateLimiter();


// ================================================================
// AUTH
// ================================================================

app.UseAuthentication();

app.UseAuthorization();


// ================================================================
// HEALTH
// ================================================================

app.MapHealthChecks(
    "/health");


// ================================================================
// CONTROLLERS
// ================================================================

app.MapControllers();


// ================================================================
// SIGNALR HUB
// ================================================================

app.MapHub<NotificationHub>(
    "/hubs/notifications");


// ================================================================
// RUN
// ================================================================

app.Run();