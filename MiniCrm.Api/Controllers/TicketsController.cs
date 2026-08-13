using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniCrm.Application.Common;
using MiniCrm.Application.DTOs.Tickets;
using MiniCrm.Application.Interfaces.Services;

namespace MiniCrm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = AppRoles.Customer)]
public class TicketsController : ControllerBase
{
    private readonly ITicketService
        _ticketService;


    public TicketsController(
        ITicketService ticketService)
    {
        _ticketService =
            ticketService;
    }


    // ============================================================
    // GET MY TICKETS
    // GET /api/Tickets
    // ============================================================

    [HttpGet]
    public async Task<IActionResult> GetMyTickets(
        CancellationToken cancellationToken)
    {
        var userId =
            GetCurrentUserId();

        var result =
            await _ticketService.GetMyTicketsAsync(
                userId,
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // CREATE
    // POST /api/Tickets
    // ============================================================

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateTicketRequestDto request,
        CancellationToken cancellationToken)
    {
        var userId =
            GetCurrentUserId();

        var result =
            await _ticketService.CreateAsync(
                userId,
                request,
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // GET DETAIL
    // GET /api/Tickets/{id}
    // ============================================================

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var userId =
            GetCurrentUserId();

        var result =
            await _ticketService.GetMyTicketAsync(
                userId,
                id,
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // ADD CUSTOMER MESSAGE
    // POST /api/Tickets/{id}/messages
    // ============================================================

    [HttpPost("{id:int}/messages")]
    public async Task<IActionResult> AddMessage(
        int id,
        [FromBody] AddTicketMessageRequestDto request,
        CancellationToken cancellationToken)
    {
        var userId =
            GetCurrentUserId();

        var result =
            await _ticketService
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