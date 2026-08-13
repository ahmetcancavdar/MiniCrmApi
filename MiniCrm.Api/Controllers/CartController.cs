using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniCrm.Application.Common;
using MiniCrm.Application.DTOs.Cart;
using MiniCrm.Application.Interfaces.Services;

namespace MiniCrm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = AppRoles.Customer)]
public class CartController : ControllerBase
{
    private readonly ICartService
        _cartService;

    public CartController(
        ICartService cartService)
    {
        _cartService =
            cartService;
    }


    // ============================================================
    // GET CART
    // GET /api/Cart
    // ============================================================

    [HttpGet]
    public async Task<IActionResult> Get(
        CancellationToken cancellationToken)
    {
        var userId =
            GetCurrentUserId();

        var result =
            await _cartService.GetAsync(
                userId,
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // ADD ITEM
    // POST /api/Cart/items
    // ============================================================

    [HttpPost("items")]
    public async Task<IActionResult> AddItem(
        [FromBody] AddCartItemRequestDto request,
        CancellationToken cancellationToken)
    {
        var userId =
            GetCurrentUserId();

        var result =
            await _cartService.AddItemAsync(
                userId,
                request,
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // UPDATE QUANTITY
    // PUT /api/Cart/items/{productId}
    // ============================================================

    [HttpPut("items/{productId:int}")]
    public async Task<IActionResult> UpdateQuantity(
        int productId,
        [FromBody] UpdateCartItemQuantityRequestDto request,
        CancellationToken cancellationToken)
    {
        var userId =
            GetCurrentUserId();

        var result =
            await _cartService.UpdateQuantityAsync(
                userId,
                productId,
                request,
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // REMOVE ITEM
    // DELETE /api/Cart/items/{productId}
    // ============================================================

    [HttpDelete("items/{productId:int}")]
    public async Task<IActionResult> RemoveItem(
        int productId,
        CancellationToken cancellationToken)
    {
        var userId =
            GetCurrentUserId();

        var result =
            await _cartService.RemoveItemAsync(
                userId,
                productId,
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // CLEAR CART
    // DELETE /api/Cart
    // ============================================================

    [HttpDelete]
    public async Task<IActionResult> Clear(
        CancellationToken cancellationToken)
    {
        var userId =
            GetCurrentUserId();

        var result =
            await _cartService.ClearAsync(
                userId,
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // CURRENT USER ID
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