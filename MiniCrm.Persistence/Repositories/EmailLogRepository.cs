using Microsoft.EntityFrameworkCore;
using MiniCrm.Application.Interfaces.Repositories;
using MiniCrm.Domain.Entities;
using MiniCrm.Persistence.Context;

namespace MiniCrm.Persistence.Repositories;

public sealed class EmailLogRepository
    : IEmailLogRepository
{
    private readonly AppDbContext _context;

    public EmailLogRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<EmailLog?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return _context.EmailLogs
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public Task<List<EmailLog>> GetByCustomerIdAsync(
        int customerId,
        CancellationToken cancellationToken = default)
    {
        return _context.EmailLogs
            .AsNoTracking()
            .Where(x => x.CustomerId == customerId)
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        EmailLog emailLog,
        CancellationToken cancellationToken = default)
    {
        await _context.EmailLogs.AddAsync(
            emailLog,
            cancellationToken);
    }
}