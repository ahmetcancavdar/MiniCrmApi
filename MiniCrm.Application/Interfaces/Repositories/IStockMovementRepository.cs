using MiniCrm.Domain.Entities;

namespace MiniCrm.Application.Interfaces.Repositories;

public interface IStockMovementRepository
{
    Task<List<StockMovement>> GetByProductIdAsync(
        int productId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        StockMovement stockMovement,
        CancellationToken cancellationToken = default);
}