using MiniCrm.Domain.Entities;

namespace MiniCrm.Application.Interfaces.Repositories;

public interface IComplaintRepository
{
    Task<Complaint?> GetByIdAsync(
        int complaintId,
        CancellationToken cancellationToken = default);

    Task<Complaint?> GetWithDetailsAsync(
        int complaintId,
        CancellationToken cancellationToken = default);

    Task<List<Complaint>> GetByCustomerIdAsync(
        int customerId,
        CancellationToken cancellationToken = default);

    Task<List<Complaint>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Complaint complaint,
        CancellationToken cancellationToken = default);
}