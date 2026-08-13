using MiniCrm.Domain.Entities;

namespace MiniCrm.Application.Interfaces.Repositories;

public interface ITicketRepository
{
    Task<Ticket?> GetByIdAsync(
        int ticketId,
        CancellationToken cancellationToken = default);

    Task<Ticket?> GetWithDetailsAsync(
        int ticketId,
        CancellationToken cancellationToken = default);

    Task<List<Ticket>> GetByCustomerIdAsync(
        int customerId,
        CancellationToken cancellationToken = default);

    Task<List<Ticket>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Ticket ticket,
        CancellationToken cancellationToken = default);
}