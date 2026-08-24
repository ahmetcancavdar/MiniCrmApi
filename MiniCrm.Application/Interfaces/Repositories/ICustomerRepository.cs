using MiniCrm.Domain.Entities;

namespace MiniCrm.Application.Interfaces.Repositories;

public interface ICustomerRepository
{
	Task<Customer?> GetByIdAsync(
		int id,
		CancellationToken cancellationToken = default);

	Task<Customer?> GetByUserIdAsync(
		Guid userId,
		CancellationToken cancellationToken = default);

	Task<Customer?> GetByEmailAsync(
		string email,
		CancellationToken cancellationToken = default);

	Task<List<Customer>> GetAllAsync(
		CancellationToken cancellationToken = default);

	Task AddAsync(
		Customer customer,
		CancellationToken cancellationToken = default);
}