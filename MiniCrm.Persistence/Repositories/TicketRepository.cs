using Microsoft.EntityFrameworkCore;
using MiniCrm.Application.Interfaces.Repositories;
using MiniCrm.Domain.Entities;
using MiniCrm.Persistence.Context;

namespace MiniCrm.Persistence.Repositories;

public sealed class TicketRepository
    : ITicketRepository
{
    private readonly AppDbContext _context;

    public TicketRepository(
        AppDbContext context)
    {
        _context =
            context;
    }


    public Task<Ticket?> GetByIdAsync(
        int ticketId,
        CancellationToken cancellationToken = default)
    {
        return _context.Tickets
            .FirstOrDefaultAsync(
                x => x.Id == ticketId,
                cancellationToken);
    }


    public Task<Ticket?> GetWithDetailsAsync(
        int ticketId,
        CancellationToken cancellationToken = default)
    {
        return _context.Tickets
            .Include(x => x.Customer)
            .Include(x => x.Order)
            .Include(x => x.Messages)
            .FirstOrDefaultAsync(
                x => x.Id == ticketId,
                cancellationToken);
    }


    public Task<List<Ticket>> GetByCustomerIdAsync(
        int customerId,
        CancellationToken cancellationToken = default)
    {
        return _context.Tickets
            .AsNoTracking()
            .Where(x =>
                x.CustomerId == customerId)
            .OrderByDescending(x =>
                x.CreatedAtUtc)
            .ToListAsync(
                cancellationToken);
    }


    public Task<List<Ticket>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return _context.Tickets
            .AsNoTracking()
            .Include(x => x.Customer)
            .Include(x => x.Order)
            .OrderByDescending(x =>
                x.CreatedAtUtc)
            .ToListAsync(
                cancellationToken);
    }


    public async Task AddAsync(
        Ticket ticket,
        CancellationToken cancellationToken = default)
    {
        await _context.Tickets.AddAsync(
            ticket,
            cancellationToken);
    }
}