using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniCrm.Application.Common;
using MiniCrm.Application.DTOs.Addresses;
using MiniCrm.Application.Interfaces.Services;

namespace MiniCrm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = AppRoles.Customer)]
public class AddressesController : ControllerBase
{
    private readonly ICustomerAddressService
        _addressService;

    public AddressesController(
        ICustomerAddressService addressService)
    {
        _addressService =
            addressService;
    }


    // ============================================================
    // GET ALL
    // GET /api/Addresses
    // ============================================================

    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var userId =
            GetCurrentUserId();

        var result =
            await _addressService.GetAllAsync(
                userId,
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // CREATE
    // POST /api/Addresses
    // ============================================================

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateAddressRequestDto request,
        CancellationToken cancellationToken)
    {
        var userId =
            GetCurrentUserId();

        var result =
            await _addressService.CreateAsync(
                userId,
                request,
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // UPDATE
    // PUT /api/Addresses/{id}
    // ============================================================

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateAddressRequestDto request,
        CancellationToken cancellationToken)
    {
        var userId =
            GetCurrentUserId();

        var result =
            await _addressService.UpdateAsync(
                userId,
                id,
                request,
                cancellationToken);

        return Ok(result);
    }


    // ============================================================
    // DELETE
    // DELETE /api/Addresses/{id}
    // ============================================================

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        var userId =
            GetCurrentUserId();

        await _addressService.DeleteAsync(
            userId,
            id,
            cancellationToken);

        return NoContent();
    }


    // ============================================================
    // SET DEFAULT
    // POST /api/Addresses/{id}/default
    // ============================================================

    [HttpPost("{id:int}/default")]
    public async Task<IActionResult> SetDefault(
        int id,
        CancellationToken cancellationToken)
    {
        var userId =
            GetCurrentUserId();

        var result =
            await _addressService.SetDefaultAsync(
                userId,
                id,
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