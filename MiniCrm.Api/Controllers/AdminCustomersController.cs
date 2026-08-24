using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniCrm.Application.Common;
using MiniCrm.Application.Interfaces.Services;

namespace MiniCrm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = AppRoles.Admin)]
public class AdminCustomersController : ControllerBase
{
    private readonly ICustomerProfileService
        _profileService;

    public AdminCustomersController(
        ICustomerProfileService profileService)
    {
        _profileService =
            profileService;
    }


    // ============================================================
    // GET ALL CUSTOMERS
    // GET /api/AdminCustomers
    // ============================================================

    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var result =
            await _profileService
                .GetAllForAdminAsync(
                    cancellationToken);

        return Ok(result);
    }
}
