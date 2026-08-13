using Microsoft.EntityFrameworkCore;
using MiniCrm.Application.Interfaces.Repositories;
using MiniCrm.Domain.Entities;
using MiniCrm.Persistence.Context;

namespace MiniCrm.Persistence.Repositories;

public sealed class CartRepository
    : ICartRepository
{
    private readonly AppDbContext _context;

    public CartRepository(
        AppDbContext context)
    {
        _context = context;
    }

    public Task<Cart?> GetByCustomerIdAsync(
        int customerId,
        CancellationToken cancellationToken = default)
    {
        return _context.Carts
            .FirstOrDefaultAsync(
                x => x.CustomerId == customerId,
                cancellationToken);
    }

    public Task<Cart?> GetWithItemsAsync(
        int customerId,
        CancellationToken cancellationToken = default)
    {
        return _context.Carts
            .Include(x => x.Items)
                .ThenInclude(x => x.Product)
                    .ThenInclude(x => x.Category)
            .FirstOrDefaultAsync(
                x => x.CustomerId == customerId,
                cancellationToken);
    }

    public async Task AddAsync(
        Cart cart,
        CancellationToken cancellationToken = default)
    {
        await _context.Carts.AddAsync(
            cart,
            cancellationToken);
    }
}