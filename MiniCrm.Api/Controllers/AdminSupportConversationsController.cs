using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniCrm.Application.Common;
using MiniCrm.Application.DTOs.Support;
using MiniCrm.Application.Interfaces.Services;

namespace MiniCrm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = AppRoles.Admin)]
public class AdminSupportConversationsController
    : ControllerBase
{
    private readonly ISupportConversationService
        _supportService;


    public AdminSupportConversationsController(
        ISupportConversationService supportService)
    {
        _supportService =
            supportService;
    }


    // ============================================================
    // LIST
    // GET /api/AdminSupportConversations
    // ============================================================

    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var result =
            await _supportService
                .GetAllForAdminAsync(
                    cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // DETAIL
    // GET /api/AdminSupportConversations/{id}
    // ============================================================

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var result =
            await _supportService
                .GetAdminConversationAsync(
                    id,
                    cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // ADMIN MESSAGE
    // POST /api/AdminSupportConversations/{id}/messages
    // ============================================================

    [HttpPost("{id:int}/messages")]
    public async Task<IActionResult> AddMessage(
        int id,
        [FromBody] AddSupportMessageRequestDto request,
        CancellationToken cancellationToken)
    {
        var adminUserId =
            GetCurrentUserId();

        var result =
            await _supportService
                .AddAdminMessageAsync(
                    adminUserId,
                    id,
                    request,
                    cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // CLOSE
    // POST /api/AdminSupportConversations/{id}/close
    // ============================================================

    [HttpPost("{id:int}/close")]
    public async Task<IActionResult> Close(
        int id,
        CancellationToken cancellationToken)
    {
        var result =
            await _supportService.CloseAsync(
                id,
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // ADMIN ID
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
                "Invalid admin identity.");
        }

        return userId;
    }
}