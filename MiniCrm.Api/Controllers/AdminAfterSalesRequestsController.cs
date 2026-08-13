using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniCrm.Application.Common;
using MiniCrm.Application.DTOs.AfterSales;
using MiniCrm.Application.Interfaces.Services;

namespace MiniCrm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = AppRoles.Admin)]
public class AdminAfterSalesRequestsController
    : ControllerBase
{
    private readonly IAfterSalesRequestService
        _afterSalesService;


    public AdminAfterSalesRequestsController(
        IAfterSalesRequestService afterSalesService)
    {
        _afterSalesService =
            afterSalesService;
    }


    // ============================================================
    // LIST
    // GET /api/AdminAfterSalesRequests
    // ============================================================

    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var result =
            await _afterSalesService
                .GetAllForAdminAsync(
                    cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // DETAIL
    // GET /api/AdminAfterSalesRequests/{id}
    // ============================================================

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var result =
            await _afterSalesService
                .GetAdminRequestAsync(
                    id,
                    cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // REVIEW
    // POST /api/AdminAfterSalesRequests/{id}/review
    // ============================================================

    [HttpPost("{id:int}/review")]
    public async Task<IActionResult> StartReview(
        int id,
        CancellationToken cancellationToken)
    {
        var result =
            await _afterSalesService
                .StartReviewAsync(
                    id,
                    cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // APPROVE
    // POST /api/AdminAfterSalesRequests/{id}/approve
    // ============================================================

    [HttpPost("{id:int}/approve")]
    public async Task<IActionResult> Approve(
        int id,
        [FromBody] AfterSalesDecisionRequestDto request,
        CancellationToken cancellationToken)
    {
        var result =
            await _afterSalesService
                .ApproveAsync(
                    id,
                    request,
                    cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // REJECT
    // POST /api/AdminAfterSalesRequests/{id}/reject
    // ============================================================

    [HttpPost("{id:int}/reject")]
    public async Task<IActionResult> Reject(
        int id,
        [FromBody] AfterSalesDecisionRequestDto request,
        CancellationToken cancellationToken)
    {
        var result =
            await _afterSalesService
                .RejectAsync(
                    id,
                    request,
                    cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // COMPLETE
    // POST /api/AdminAfterSalesRequests/{id}/complete
    // ============================================================

    [HttpPost("{id:int}/complete")]
    public async Task<IActionResult> Complete(
        int id,
        CancellationToken cancellationToken)
    {
        var result =
            await _afterSalesService
                .CompleteAsync(
                    id,
                    cancellationToken);

        return Ok(result);
    }
}