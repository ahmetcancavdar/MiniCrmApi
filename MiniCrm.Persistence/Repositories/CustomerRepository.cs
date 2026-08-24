using Microsoft.EntityFrameworkCore;
using MiniCrm.Application.Interfaces.Repositories;
using MiniCrm.Domain.Entities;
using MiniCrm.Persistence.Context;

namespace MiniCrm.Persistence.Repositories;

public sealed class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Customer?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return _context.Customers
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public Task<Customer?> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return _context.Customers
            .FirstOrDefaultAsync(
                x => x.UserId == userId,
                cancellationToken);
    }

    public Task<Customer?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        return _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Email == email,
                cancellationToken);
    }

    public Task<List<Customer>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return _context.Customers
            .AsNoTracking()
            .OrderBy(x => x.FullName)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        Customer customer,
        CancellationToken cancellationToken = default)
    {
        await _context.Customers.AddAsync(
            customer,
            cancellationToken);
    }
}