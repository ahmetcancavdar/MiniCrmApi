using MiniCrmApi.Domain;
using MiniCrmApi.DTOs;
using MiniCrmApi.Repositories;

namespace MiniCrmApi.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<List<CustomerDto>> GetAllAsync()
    {
        var customers = await _customerRepository.GetAllActiveAsync();

        return customers.Select(MapToDto).ToList();
    }

    public async Task<CustomerDto?> GetByIdAsync(int id)
    {
        var customer = await _customerRepository.GetActiveByIdAsync(id);

        if (customer is null)
        {
            return null;
        }

        return MapToDto(customer);
    }

    public async Task<CustomerDto> CreateAsync(CreateCustomerDto dto)
    {
        var customer = new Customer
        {
            FullName = dto.FullName,
            Email = dto.Email,
            Phone = dto.Phone,
            CompanyName = dto.CompanyName
        };

        await _customerRepository.AddAsync(customer);
        await _customerRepository.SaveChangesAsync();

        return MapToDto(customer);
    }

    public async Task<bool> UpdateAsync(int id, UpdateCustomerDto dto)
    {
        var customer = await _customerRepository.GetActiveByIdAsync(id);

        if (customer is null)
        {
            return false;
        }

        customer.FullName = dto.FullName;
        customer.Email = dto.Email;
        customer.Phone = dto.Phone;
        customer.CompanyName = dto.CompanyName;
        customer.UpdatedDate = DateTime.Now;

        _customerRepository.Update(customer);
        await _customerRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var customer = await _customerRepository.GetActiveByIdAsync(id);

        if (customer is null)
        {
            return false;
        }

        customer.IsDeleted = true;
        customer.UpdatedDate = DateTime.Now;

        _customerRepository.Update(customer);
        await _customerRepository.SaveChangesAsync();

        return true;
    }

    private static CustomerDto MapToDto(Customer customer)
    {
        return new CustomerDto
        {
            Id = customer.Id,
            FullName = customer.FullName,
            Email = customer.Email,
            Phone = customer.Phone,
            CompanyName = customer.CompanyName,
            CreatedDate = customer.CreatedDate,
            UpdatedDate = customer.UpdatedDate
        };
    }
}