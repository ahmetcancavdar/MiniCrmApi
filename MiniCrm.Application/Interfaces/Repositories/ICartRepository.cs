using MiniCrm.Domain.Entities;

namespace MiniCrm.Application.Interfaces.Repositories;

public interface ICartRepository
{
    Task<Cart?> GetByCustomerIdAsync(
        int customerId,
        CancellationToken cancellationToken = default);

    Task<Cart?> GetWithItemsAsync(
        int customerId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Cart cart,
        CancellationToken cancellationToken = default);
}