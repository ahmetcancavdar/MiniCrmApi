using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniCrm.Application.Common;
using MiniCrm.Application.DTOs.AfterSales;
using MiniCrm.Application.Interfaces.Services;

namespace MiniCrm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = AppRoles.Customer)]
public class AfterSalesRequestsController
    : ControllerBase
{
    private readonly IAfterSalesRequestService
        _afterSalesService;


    public AfterSalesRequestsController(
        IAfterSalesRequestService afterSalesService)
    {
        _afterSalesService =
            afterSalesService;
    }


    // ============================================================
    // LIST
    // GET /api/AfterSalesRequests
    // ============================================================

    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var userId =
            GetCurrentUserId();

        var result =
            await _afterSalesService
                .GetMyRequestsAsync(
                    userId,
                    cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // CREATE
    // POST /api/AfterSalesRequests
    // ============================================================

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateAfterSalesRequestDto request,
        CancellationToken cancellationToken)
    {
        var userId =
            GetCurrentUserId();

        var result =
            await _afterSalesService
                .CreateAsync(
                    userId,
                    request,
                    cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // DETAIL
    // GET /api/AfterSalesRequests/{id}
    // ============================================================

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var userId =
            GetCurrentUserId();

        var result =
            await _afterSalesService
                .GetMyRequestAsync(
                    userId,
                    id,
                    cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // CANCEL
    // POST /api/AfterSalesRequests/{id}/cancel
    // ============================================================

    [HttpPost("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(
        int id,
        CancellationToken cancellationToken)
    {
        var userId =
            GetCurrentUserId();

        var result =
            await _afterSalesService
                .CancelAsync(
                    userId,
                    id,
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