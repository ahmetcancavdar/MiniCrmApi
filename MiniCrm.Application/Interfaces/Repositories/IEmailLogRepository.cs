using MiniCrm.Domain.Entities;

namespace MiniCrm.Application.Interfaces.Repositories;

public interface IEmailLogRepository
{
    Task<List<EmailLog>> GetByCustomerIdAsync(
        int customerId,
        CancellationToken cancellationToken = default);

    Task<EmailLog?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        EmailLog emailLog,
        CancellationToken cancellationToken = default);
}