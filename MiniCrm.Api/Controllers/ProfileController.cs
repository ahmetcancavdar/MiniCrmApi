using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniCrm.Application.Common;
using MiniCrm.Application.DTOs.Profile;
using MiniCrm.Application.Interfaces.Services;

namespace MiniCrm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = AppRoles.Customer)]
public class ProfileController : ControllerBase
{
    private readonly ICustomerProfileService
        _profileService;

    public ProfileController(
        ICustomerProfileService profileService)
    {
        _profileService =
            profileService;
    }


    // ============================================================
    // GET PROFILE
    // GET /api/Profile
    // ============================================================

    [HttpGet]
    public async Task<IActionResult> Get(
        CancellationToken cancellationToken)
    {
        var userId =
            GetCurrentUserId();

        var result =
            await _profileService.GetAsync(
                userId,
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // UPDATE PROFILE
    // PUT /api/Profile
    // ============================================================

    [HttpPut]
    public async Task<IActionResult> Update(
        [FromBody] UpdateProfileRequestDto request,
        CancellationToken cancellationToken)
    {
        var userId =
            GetCurrentUserId();

        var result =
            await _profileService.UpdateAsync(
                userId,
                request,
                cancellationToken);

        return Ok(result);
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