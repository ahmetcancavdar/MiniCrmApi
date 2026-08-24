using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniCrm.Application.Common;
using MiniCrm.Application.DTOs.Orders;
using MiniCrm.Application.Interfaces.Services;

namespace MiniCrm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = AppRoles.Customer)]
public class OrdersController : ControllerBase
{
    private readonly IOrderService
        _orderService;

    public OrdersController(
        IOrderService orderService)
    {
        _orderService =
            orderService;
    }


    // ============================================================
    // GET MY ORDERS
    // GET /api/Orders
    // ============================================================

    [HttpGet]
    public async Task<IActionResult> GetMyOrders(
        CancellationToken cancellationToken)
    {
        var userId =
            GetCurrentUserId();

        var result =
            await _orderService.GetMyOrdersAsync(
                userId,
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // CHECKOUT
    // POST /api/Orders/checkout
    // ============================================================

    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout(
        [FromBody] CheckoutOrderRequestDto request,
        CancellationToken cancellationToken)
    {
        var userId =
            GetCurrentUserId();

        var result =
            await _orderService.CheckoutAsync(
                userId,
                request,
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // VERIFY
    // POST /api/Orders/{id}/verify
    // ============================================================

    [HttpPost("{id:int}/verify")]
    public async Task<IActionResult> Verify(
        int id,
        [FromBody] VerifyOrderRequestDto request,
        CancellationToken cancellationToken)
    {
        var userId =
            GetCurrentUserId();

        var result =
            await _orderService.VerifyAsync(
                userId,
                id,
                request,
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // RESEND VERIFICATION
    // POST /api/Orders/{id}/resend-verification
    // ============================================================

    [HttpPost("{id:int}/resend-verification")]
    public async Task<IActionResult> ResendVerification(
        int id,
        CancellationToken cancellationToken)
    {
        var userId =
            GetCurrentUserId();

        var result =
            await _orderService
                .ResendVerificationCodeAsync(
                    userId,
                    id,
                    cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // GET ORDER DETAIL
    // GET /api/Orders/{id}
    // ============================================================

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var userId =
            GetCurrentUserId();

        var result =
            await _orderService.GetByIdAsync(
                userId,
                id,
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // CANCEL
    // POST /api/Orders/{id}/cancel
    // ============================================================

    [HttpPost("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(
        int id,
        [FromBody] CancelOrderRequestDto request,
        CancellationToken cancellationToken)
    {
        var userId =
            GetCurrentUserId();

        var result =
            await _orderService.CancelAsync(
                userId,
                id,
                request,
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // CURRENT CUSTOMER USER ID
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