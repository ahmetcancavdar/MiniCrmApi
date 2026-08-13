using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniCrm.Application.Common;
using MiniCrm.Application.DTOs.Categories;
using MiniCrm.Application.Interfaces.Services;

namespace MiniCrm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(
        ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // ============================================================
    // CUSTOMER + ADMIN
    // GET /api/Categories
    // ============================================================

    [HttpGet]
    public async Task<IActionResult> GetActive(
        CancellationToken cancellationToken)
    {
        var result =
            await _categoryService.GetActiveAsync(
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // ADMIN
    // GET /api/Categories/admin
    // ============================================================

    [Authorize(Roles = AppRoles.Admin)]
    [HttpGet("admin")]
    public async Task<IActionResult> GetAllForAdmin(
        CancellationToken cancellationToken)
    {
        var result =
            await _categoryService
                .GetAllForAdminAsync(
                    cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // ADMIN
    // POST /api/Categories
    // ============================================================

    [Authorize(Roles = AppRoles.Admin)]
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateCategoryRequestDto request,
        CancellationToken cancellationToken)
    {
        var result =
            await _categoryService.CreateAsync(
                request,
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // ADMIN
    // PUT /api/Categories/{id}
    // ============================================================

    [Authorize(Roles = AppRoles.Admin)]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateCategoryRequestDto request,
        CancellationToken cancellationToken)
    {
        var result =
            await _categoryService.UpdateAsync(
                id,
                request,
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // ADMIN
    // DELETE /api/Categories/{id}
    // ============================================================

    [Authorize(Roles = AppRoles.Admin)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        await _categoryService.DeleteAsync(
            id,
            cancellationToken);

        return NoContent();
    }
}