using MiniCrm.Domain.Entities;

namespace MiniCrm.Application.Interfaces.Repositories;

public interface IOrderRepository
{
	Task<Order?> GetByIdAsync(
		int id,
		CancellationToken cancellationToken = default);

	Task<Order?> GetByOrderNumberAsync(
		string orderNumber,
		CancellationToken cancellationToken = default);

	Task<Order?> GetWithDetailsAsync(
		int id,
		CancellationToken cancellationToken = default);

	Task<List<Order>> GetByCustomerIdAsync(
		int customerId,
		CancellationToken cancellationToken = default);

	Task<List<Order>> GetAllAsync(
		CancellationToken cancellationToken = default);

	Task AddAsync(
		Order order,
		CancellationToken cancellationToken = default);
}