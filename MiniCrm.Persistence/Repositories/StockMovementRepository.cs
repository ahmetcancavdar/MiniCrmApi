using Microsoft.EntityFrameworkCore;
using MiniCrm.Application.Interfaces.Repositories;
using MiniCrm.Domain.Entities;
using MiniCrm.Persistence.Context;

namespace MiniCrm.Persistence.Repositories;

public sealed class StockMovementRepository
    : IStockMovementRepository
{
    private readonly AppDbContext _context;

    public StockMovementRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<List<StockMovement>> GetByProductIdAsync(
        int productId,
        CancellationToken cancellationToken = default)
    {
        return _context.StockMovements
            .AsNoTracking()
            .Where(x => x.ProductId == productId)
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        StockMovement stockMovement,
        CancellationToken cancellationToken = default)
    {
        await _context.StockMovements.AddAsync(
            stockMovement,
            cancellationToken);
    }
}