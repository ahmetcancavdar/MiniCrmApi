using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniCrm.Application.DTOs.Auth;
using MiniCrm.Application.Interfaces.Services;

namespace MiniCrm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }


    // ============================================================
    // REGISTER
    // POST: /api/Auth/register
    // ============================================================

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequestDto request,
        CancellationToken cancellationToken)
    {
        var result =
            await _authService.RegisterAsync(
                request,
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // LOGIN
    // POST: /api/Auth/login
    // ============================================================

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequestDto request,
        CancellationToken cancellationToken)
    {
        var result =
            await _authService.LoginAsync(
                request,
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // CURRENT USER
    // GET: /api/Auth/me
    // ============================================================

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        var userId =
            User.FindFirstValue(
                ClaimTypes.NameIdentifier);

        var email =
            User.FindFirstValue("email");

        var roles =
            User.FindAll(ClaimTypes.Role)
                .Select(x => x.Value)
                .ToList();

        return Ok(new
        {
            isAuthenticated =
                User.Identity?.IsAuthenticated
                ?? false,

            userId,

            email,

            roles
        });
    }


    // ============================================================
    // CHANGE PASSWORD
    // POST: /api/Auth/change-password
    // ============================================================

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(
        [FromBody] ChangePasswordRequestDto request,
        CancellationToken cancellationToken)
    {
        await _authService.ChangePasswordAsync(
            GetCurrentUserId(),
            request,
            cancellationToken);

        return NoContent();
    }


    // ============================================================
    // CHANGE EMAIL
    // POST: /api/Auth/change-email
    // ============================================================

    [Authorize]
    [HttpPost("change-email")]
    public async Task<IActionResult> ChangeEmail(
        [FromBody] ChangeEmailRequestDto request,
        CancellationToken cancellationToken)
    {
        await _authService.ChangeEmailAsync(
            GetCurrentUserId(),
            request,
            cancellationToken);

        return NoContent();
    }


    // ============================================================
    // USER ID
    // ============================================================

    private Guid GetCurrentUserId()
    {
        var userIdValue =
            User.FindFirstValue(
                ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(
                userIdValue,
                out var userId))
        {
            throw new UnauthorizedAccessException(
                "Invalid user identity.");
        }

        return userId;
    }
}