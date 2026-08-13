using MiniCrm.Domain.Entities;

namespace MiniCrm.Application.Interfaces.Repositories;

public interface IAfterSalesRequestRepository
{
    Task<AfterSalesRequest?> GetByIdAsync(
        int requestId,
        CancellationToken cancellationToken = default);

    Task<AfterSalesRequest?> GetWithDetailsAsync(
        int requestId,
        CancellationToken cancellationToken = default);

    Task<List<AfterSalesRequest>> GetByCustomerIdAsync(
        int customerId,
        CancellationToken cancellationToken = default);

    Task<List<AfterSalesRequest>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<bool> HasActiveRequestForOrderItemAsync(
        int customerId,
        int orderItemId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        AfterSalesRequest request,
        CancellationToken cancellationToken = default);
}