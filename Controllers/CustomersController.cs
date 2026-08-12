using Microsoft.AspNetCore.Mvc;
using MiniCrmApi.DTOs;
using MiniCrmApi.Services;

namespace MiniCrmApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var customers = await _customerService.GetAllAsync();

        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var customer = await _customerService.GetByIdAsync(id);

        if (customer is null)
        {
            return NotFound("Müşteri bulunamadı.");
        }

        return Ok(customer);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCustomerDto dto)
    {
        var createdCustomer = await _customerService.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdCustomer.Id },
            createdCustomer
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateCustomerDto dto)
    {
        var isUpdated = await _customerService.UpdateAsync(id, dto);

        if (!isUpdated)
        {
            return NotFound("Müşteri bulunamadı.");
        }

        return NoContent();
    }

    [HttpDelete("{id}")]    
    public async Task<IActionResult> Delete(int id)
    {
        var isDeleted = await _customerService.DeleteAsync(id);

        if (!isDeleted)
        {
            return NotFound("Müşteri bulunamadı.");
        }

        return NoContent();
    }
}