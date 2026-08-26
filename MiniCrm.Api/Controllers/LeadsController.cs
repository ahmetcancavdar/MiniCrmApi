using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniCrm.Application.Common;
using MiniCrm.Application.DTOs.Leads;
using MiniCrm.Application.Interfaces.Services;

namespace MiniCrm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = AppRoles.Admin)]
public class LeadsController : ControllerBase
{
    private readonly ILeadService _leadService;

    public LeadsController(
        ILeadService leadService)
    {
        _leadService = leadService;
    }


    // ============================================================
    // GET /api/Leads
    // ============================================================

    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var result =
            await _leadService.GetAllAsync(
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // GET /api/Leads/{id}
    // ============================================================

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var result =
            await _leadService.GetByIdAsync(
                id,
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // POST /api/Leads
    // ============================================================

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateLeadRequestDto request,
        CancellationToken cancellationToken)
    {
        var result =
            await _leadService.CreateAsync(
                request,
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // PUT /api/Leads/{id}
    // ============================================================

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateLeadRequestDto request,
        CancellationToken cancellationToken)
    {
        var result =
            await _leadService.UpdateAsync(
                id,
                request,
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // POST /api/Leads/{id}/status
    // ============================================================

    [HttpPost("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(
        int id,
        [FromBody] UpdateLeadStatusRequestDto request,
        CancellationToken cancellationToken)
    {
        var result =
            await _leadService.UpdateStatusAsync(
                id,
                GetCurrentUserId(),
                request,
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // POST /api/Leads/{id}/notes
    // ============================================================

    [HttpPost("{id:int}/notes")]
    public async Task<IActionResult> AddNote(
        int id,
        [FromBody] AddLeadNoteRequestDto request,
        CancellationToken cancellationToken)
    {
        var result =
            await _leadService.AddNoteAsync(
                id,
                GetCurrentUserId(),
                request,
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // POST /api/Leads/{id}/convert
    // ============================================================

    [HttpPost("{id:int}/convert")]
    public async Task<IActionResult> Convert(
        int id,
        [FromBody] ConvertLeadRequestDto? request,
        CancellationToken cancellationToken)
    {
        var result =
            await _leadService.ConvertToCustomerAsync(
                id,
                request ?? new ConvertLeadRequestDto(),
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // DELETE /api/Leads/{id}
    // ============================================================

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        await _leadService.DeleteAsync(
            id,
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
