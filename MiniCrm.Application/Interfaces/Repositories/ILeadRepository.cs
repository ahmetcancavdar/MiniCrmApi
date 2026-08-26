using MiniCrm.Domain.Entities;

namespace MiniCrm.Application.Interfaces.Repositories;

public interface ILeadRepository
{
    Task<Lead?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<Lead?> GetWithDetailsAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<List<Lead>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<bool> ExistsActiveByEmailAsync(
        string email,
        int? excludeLeadId = null,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Lead lead,
        CancellationToken cancellationToken = default);
}
