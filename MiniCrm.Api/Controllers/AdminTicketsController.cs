using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniCrm.Application.Common;
using MiniCrm.Application.DTOs.Tickets;
using MiniCrm.Application.Interfaces.Services;

namespace MiniCrm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = AppRoles.Admin)]
public class AdminTicketsController : ControllerBase
{
    private readonly ITicketService
        _ticketService;


    public AdminTicketsController(
        ITicketService ticketService)
    {
        _ticketService =
            ticketService;
    }


    // ============================================================
    // GET ALL
    // GET /api/AdminTickets
    // ============================================================

    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var result =
            await _ticketService
                .GetAllForAdminAsync(
                    cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // GET DETAIL
    // GET /api/AdminTickets/{id}
    // ============================================================

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var result =
            await _ticketService
                .GetAdminTicketAsync(
                    id,
                    cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // ADMIN MESSAGE
    // POST /api/AdminTickets/{id}/messages
    // ============================================================

    [HttpPost("{id:int}/messages")]
    public async Task<IActionResult> AddMessage(
        int id,
        [FromBody] AddTicketMessageRequestDto request,
        CancellationToken cancellationToken)
    {
        var adminUserId =
            GetCurrentUserId();

        var result =
            await _ticketService
                .AddAdminMessageAsync(
                    adminUserId,
                    id,
                    request,
                    cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // START
    // POST /api/AdminTickets/{id}/start
    // ============================================================

    [HttpPost("{id:int}/start")]
    public async Task<IActionResult> Start(
        int id,
        CancellationToken cancellationToken)
    {
        var result =
            await _ticketService
                .StartProgressAsync(
                    id,
                    cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // WAIT
    // POST /api/AdminTickets/{id}/wait
    // ============================================================

    [HttpPost("{id:int}/wait")]
    public async Task<IActionResult> WaitForCustomer(
        int id,
        CancellationToken cancellationToken)
    {
        var result =
            await _ticketService
                .SetWaitingForCustomerAsync(
                    id,
                    cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // RESOLVE
    // POST /api/AdminTickets/{id}/resolve
    // ============================================================

    [HttpPost("{id:int}/resolve")]
    public async Task<IActionResult> Resolve(
        int id,
        CancellationToken cancellationToken)
    {
        var result =
            await _ticketService
                .ResolveAsync(
                    id,
                    cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // CLOSE
    // POST /api/AdminTickets/{id}/close
    // ============================================================

    [HttpPost("{id:int}/close")]
    public async Task<IActionResult> Close(
        int id,
        CancellationToken cancellationToken)
    {
        var result =
            await _ticketService
                .CloseAsync(
                    id,
                    cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // ADMIN USER ID
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