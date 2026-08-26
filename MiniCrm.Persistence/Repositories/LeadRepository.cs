using Microsoft.EntityFrameworkCore;
using MiniCrm.Application.Interfaces.Repositories;
using MiniCrm.Domain.Entities;
using MiniCrm.Domain.Enums;
using MiniCrm.Persistence.Context;

namespace MiniCrm.Persistence.Repositories;

public sealed class LeadRepository : ILeadRepository
{
    private readonly AppDbContext _context;

    public LeadRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Lead?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return _context.Leads
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public Task<Lead?> GetWithDetailsAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return _context.Leads
            .Include(x => x.LeadNotes)
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public Task<List<Lead>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return _context.Leads
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }

    public Task<bool> ExistsActiveByEmailAsync(
        string email,
        int? excludeLeadId = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();

        return _context.Leads
            .Where(x => x.Email == normalizedEmail)
            .Where(x => x.Status != LeadStatus.Lost && x.Status != LeadStatus.Converted)
            .Where(x => excludeLeadId == null || x.Id != excludeLeadId)
            .AnyAsync(cancellationToken);
    }

    public async Task AddAsync(
        Lead lead,
        CancellationToken cancellationToken = default)
    {
        await _context.Leads.AddAsync(
            lead,
            cancellationToken);
    }
}
