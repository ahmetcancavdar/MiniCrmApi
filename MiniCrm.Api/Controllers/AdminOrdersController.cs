using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniCrm.Application.Common;
using MiniCrm.Application.DTOs.Orders;
using MiniCrm.Application.Interfaces.Services;

namespace MiniCrm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = AppRoles.Admin)]
public class AdminOrdersController : ControllerBase
{
    private readonly IOrderService
        _orderService;

    public AdminOrdersController(
        IOrderService orderService)
    {
        _orderService =
            orderService;
    }


    // ============================================================
    // GET ALL ORDERS
    // GET /api/AdminOrders
    // ============================================================

    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var result =
            await _orderService
                .GetAllForAdminAsync(
                    cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // GET ORDER DETAIL
    // GET /api/AdminOrders/{id}
    // ============================================================

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var result =
            await _orderService
                .GetAdminByIdAsync(
                    id,
                    cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // PREPARING
    // POST /api/AdminOrders/{id}/prepare
    // ============================================================

    [HttpPost("{id:int}/prepare")]
    public async Task<IActionResult> Prepare(
        int id,
        CancellationToken cancellationToken)
    {
        var result =
            await _orderService
                .StartPreparingAsync(
                    id,
                    cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // SHIP
    // POST /api/AdminOrders/{id}/ship
    // ============================================================

    [HttpPost("{id:int}/ship")]
    public async Task<IActionResult> Ship(
        int id,
        CancellationToken cancellationToken)
    {
        var result =
            await _orderService
                .MarkAsShippedAsync(
                    id,
                    cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // DELIVER
    // POST /api/AdminOrders/{id}/deliver
    // ============================================================

    [HttpPost("{id:int}/deliver")]
    public async Task<IActionResult> Deliver(
        int id,
        CancellationToken cancellationToken)
    {
        var result =
            await _orderService
                .MarkAsDeliveredAsync(
                    id,
                    cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // CANCEL
    // POST /api/AdminOrders/{id}/cancel
    // ============================================================

    [HttpPost("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(
        int id,
        [FromBody] CancelOrderRequestDto request,
        CancellationToken cancellationToken)
    {
        var result =
            await _orderService
                .CancelByAdminAsync(
                    id,
                    request,
                    cancellationToken);

        return Ok(result);
    }
}