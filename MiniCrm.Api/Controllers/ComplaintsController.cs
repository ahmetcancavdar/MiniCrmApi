using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniCrm.Application.Common;
using MiniCrm.Application.DTOs.Complaints;
using MiniCrm.Application.Interfaces.Services;

namespace MiniCrm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = AppRoles.Customer)]
public class ComplaintsController : ControllerBase
{
    private readonly IComplaintService
        _complaintService;


    public ComplaintsController(
        IComplaintService complaintService)
    {
        _complaintService =
            complaintService;
    }


    // ============================================================
    // GET MY COMPLAINTS
    // GET /api/Complaints
    // ============================================================

    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var userId =
            GetCurrentUserId();

        var result =
            await _complaintService
                .GetMyComplaintsAsync(
                    userId,
                    cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // CREATE
    // POST /api/Complaints
    // ============================================================

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateComplaintRequestDto request,
        CancellationToken cancellationToken)
    {
        var userId =
            GetCurrentUserId();

        var result =
            await _complaintService.CreateAsync(
                userId,
                request,
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // GET DETAIL
    // GET /api/Complaints/{id}
    // ============================================================

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var userId =
            GetCurrentUserId();

        var result =
            await _complaintService
                .GetMyComplaintAsync(
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