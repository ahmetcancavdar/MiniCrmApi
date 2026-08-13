using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using MiniCrm.Api.ErrorHandling;
using MiniCrm.Api.Middleware;
using MiniCrm.Api.OpenApi;
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
        });


builder.Services.AddAuthorization();


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
// IDENTITY SEED
// ================================================================

await IdentitySeeder.SeedAsync(
    app.Services,
    app.Configuration);


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
// RUN
// ================================================================

app.Run();