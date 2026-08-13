using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniCrm.Application.Common;
using MiniCrm.Application.DTOs.Complaints;
using MiniCrm.Application.Interfaces.Services;

namespace MiniCrm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = AppRoles.Admin)]
public class AdminComplaintsController : ControllerBase
{
    private readonly IComplaintService
        _complaintService;


    public AdminComplaintsController(
        IComplaintService complaintService)
    {
        _complaintService =
            complaintService;
    }


    // ============================================================
    // GET ALL
    // GET /api/AdminComplaints
    // ============================================================

    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var result =
            await _complaintService
                .GetAllForAdminAsync(
                    cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // GET DETAIL
    // GET /api/AdminComplaints/{id}
    // ============================================================

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var result =
            await _complaintService
                .GetAdminComplaintAsync(
                    id,
                    cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // START REVIEW
    // POST /api/AdminComplaints/{id}/review
    // ============================================================

    [HttpPost("{id:int}/review")]
    public async Task<IActionResult> StartReview(
        int id,
        CancellationToken cancellationToken)
    {
        var result =
            await _complaintService
                .StartReviewAsync(
                    id,
                    cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // RESOLVE
    // POST /api/AdminComplaints/{id}/resolve
    // ============================================================

    [HttpPost("{id:int}/resolve")]
    public async Task<IActionResult> Resolve(
        int id,
        [FromBody] ResolveComplaintRequestDto request,
        CancellationToken cancellationToken)
    {
        var result =
            await _complaintService
                .ResolveAsync(
                    id,
                    request,
                    cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // REJECT
    // POST /api/AdminComplaints/{id}/reject
    // ============================================================

    [HttpPost("{id:int}/reject")]
    public async Task<IActionResult> Reject(
        int id,
        [FromBody] RejectComplaintRequestDto request,
        CancellationToken cancellationToken)
    {
        var result =
            await _complaintService
                .RejectAsync(
                    id,
                    request,
                    cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // CLOSE
    // POST /api/AdminComplaints/{id}/close
    // ============================================================

    [HttpPost("{id:int}/close")]
    public async Task<IActionResult> Close(
        int id,
        CancellationToken cancellationToken)
    {
        var result =
            await _complaintService
                .CloseAsync(
                    id,
                    cancellationToken);

        return Ok(result);
    }
}