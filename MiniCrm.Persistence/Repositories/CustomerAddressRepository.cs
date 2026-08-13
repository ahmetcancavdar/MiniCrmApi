using Microsoft.EntityFrameworkCore;
using MiniCrm.Application.Interfaces.Repositories;
using MiniCrm.Domain.Entities;
using MiniCrm.Persistence.Context;

namespace MiniCrm.Persistence.Repositories;

public sealed class CustomerAddressRepository
    : ICustomerAddressRepository
{
    private readonly AppDbContext _context;

    public CustomerAddressRepository(
        AppDbContext context)
    {
        _context = context;
    }

    public Task<List<CustomerAddress>>
        GetByCustomerIdAsync(
            int customerId,
            CancellationToken cancellationToken = default)
    {
        return _context.CustomerAddresses
            .Where(x =>
                x.CustomerId == customerId)
            .OrderByDescending(x =>
                x.IsDefault)
            .ThenBy(x =>
                x.Id)
            .ToListAsync(
                cancellationToken);
    }

    public Task<CustomerAddress?> GetByIdAsync(
        int addressId,
        CancellationToken cancellationToken = default)
    {
        return _context.CustomerAddresses
            .FirstOrDefaultAsync(
                x => x.Id == addressId,
                cancellationToken);
    }

    public async Task AddAsync(
        CustomerAddress address,
        CancellationToken cancellationToken = default)
    {
        await _context.CustomerAddresses
            .AddAsync(
                address,
                cancellationToken);
    }

    public void Remove(
        CustomerAddress address)
    {
        _context.CustomerAddresses
            .Remove(address);
    }
}