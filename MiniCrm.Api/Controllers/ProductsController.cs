using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniCrm.Application.Common;
using MiniCrm.Application.DTOs.Products;
using MiniCrm.Application.Interfaces.Services;

namespace MiniCrm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(
        IProductService productService)
    {
        _productService = productService;
    }


    // ============================================================
    // CUSTOMER + ADMIN
    // GET /api/Products
    // ============================================================

    [HttpGet]
    public async Task<IActionResult> GetActive(
        CancellationToken cancellationToken)
    {
        var result =
            await _productService.GetActiveAsync(
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // CUSTOMER + ADMIN
    // GET /api/Products/{id}
    // ============================================================

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var result =
            await _productService.GetActiveByIdAsync(
                id,
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // ADMIN
    // GET /api/Products/admin
    // ============================================================

    [Authorize(Roles = AppRoles.Admin)]
    [HttpGet("admin")]
    public async Task<IActionResult> GetAllForAdmin(
        CancellationToken cancellationToken)
    {
        var result =
            await _productService
                .GetAllForAdminAsync(
                    cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // ADMIN
    // POST /api/Products
    // ============================================================

    [Authorize(Roles = AppRoles.Admin)]
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateProductRequestDto request,
        CancellationToken cancellationToken)
    {
        var result =
            await _productService.CreateAsync(
                request,
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // ADMIN
    // PUT /api/Products/{id}
    // ============================================================

    [Authorize(Roles = AppRoles.Admin)]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateProductRequestDto request,
        CancellationToken cancellationToken)
    {
        var result =
            await _productService.UpdateAsync(
                id,
                request,
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // ADMIN
    // DELETE /api/Products/{id}
    // ============================================================

    [Authorize(Roles = AppRoles.Admin)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        await _productService.DeleteAsync(
            id,
            cancellationToken);

        return NoContent();
    }


    // ============================================================
    // ADMIN
    // POST /api/Products/{id}/stock
    // ============================================================

    [Authorize(Roles = AppRoles.Admin)]
    [HttpPost("{id:int}/stock")]
    public async Task<IActionResult> AdjustStock(
        int id,
        [FromBody] AdjustStockRequestDto request,
        CancellationToken cancellationToken)
    {
        var result =
            await _productService.AdjustStockAsync(
                id,
                request,
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // ADMIN
    // GET /api/Products/{id}/stock-movements
    // ============================================================

    [Authorize(Roles = AppRoles.Admin)]
    [HttpGet("{id:int}/stock-movements")]
    public async Task<IActionResult> GetStockMovements(
        int id,
        CancellationToken cancellationToken)
    {
        var result =
            await _productService
                .GetStockMovementsAsync(
                    id,
                    cancellationToken);

        return Ok(result);
    }
}