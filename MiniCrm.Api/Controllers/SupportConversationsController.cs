using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniCrm.Application.Common;
using MiniCrm.Application.DTOs.Support;
using MiniCrm.Application.Interfaces.Services;

namespace MiniCrm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = AppRoles.Customer)]
public class SupportConversationsController
    : ControllerBase
{
    private readonly ISupportConversationService
        _supportService;


    public SupportConversationsController(
        ISupportConversationService supportService)
    {
        _supportService =
            supportService;
    }


    // ============================================================
    // LIST
    // GET /api/SupportConversations
    // ============================================================

    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var userId =
            GetCurrentUserId();

        var result =
            await _supportService
                .GetMyConversationsAsync(
                    userId,
                    cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // CREATE
    // POST /api/SupportConversations
    // ============================================================

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateSupportConversationRequestDto request,
        CancellationToken cancellationToken)
    {
        var userId =
            GetCurrentUserId();

        var result =
            await _supportService.CreateAsync(
                userId,
                request,
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // DETAIL
    // GET /api/SupportConversations/{id}
    // ============================================================

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var userId =
            GetCurrentUserId();

        var result =
            await _supportService
                .GetMyConversationAsync(
                    userId,
                    id,
                    cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // MESSAGE
    // POST /api/SupportConversations/{id}/messages
    // ============================================================

    [HttpPost("{id:int}/messages")]
    public async Task<IActionResult> AddMessage(
        int id,
        [FromBody] AddSupportMessageRequestDto request,
        CancellationToken cancellationToken)
    {
        var userId =
            GetCurrentUserId();

        var result =
            await _supportService
                .AddCustomerMessageAsync(
                    userId,
                    id,
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