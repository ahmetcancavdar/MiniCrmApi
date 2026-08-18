using Microsoft.EntityFrameworkCore;
using MiniCrm.Application.Interfaces.Repositories;
using MiniCrm.Domain.Entities;
using MiniCrm.Persistence.Context;

namespace MiniCrm.Persistence.Repositories;

public sealed class SupportConversationRepository
    : ISupportConversationRepository
{
    private readonly AppDbContext
        _context;


    public SupportConversationRepository(
        AppDbContext context)
    {
        _context =
            context;
    }


    // ============================================================
    // DETAIL
    // ============================================================

    public Task<SupportConversation?> GetWithDetailsAsync(
        int conversationId,
        CancellationToken cancellationToken = default)
    {
        return _context.SupportConversations
            .Include(x => x.Customer)
            .Include(x => x.Order)
            .Include(x => x.Messages)
            .FirstOrDefaultAsync(
                x => x.Id == conversationId,
                cancellationToken);
    }


    // ============================================================
    // CUSTOMER LIST
    // ============================================================

    public Task<List<SupportConversation>>
        GetByCustomerIdAsync(
            int customerId,
            CancellationToken cancellationToken = default)
    {
        return _context.SupportConversations
            .AsNoTracking()
            .Include(x => x.Order)
            .Where(x =>
                x.CustomerId == customerId)
            .OrderByDescending(x =>
                x.UpdatedAtUtc ?? x.CreatedAtUtc)
            .ToListAsync(
                cancellationToken);
    }


    // ============================================================
    // ADMIN LIST
    // ============================================================

    public Task<List<SupportConversation>>
        GetAllAsync(
            CancellationToken cancellationToken = default)
    {
        return _context.SupportConversations
            .AsNoTracking()
            .Include(x => x.Customer)
            .Include(x => x.Order)
            .OrderByDescending(x =>
                x.UpdatedAtUtc ?? x.CreatedAtUtc)
            .ToListAsync(
                cancellationToken);
    }


    // ============================================================
    // ADD
    // ============================================================

    public async Task AddAsync(
        SupportConversation conversation,
        CancellationToken cancellationToken = default)
    {
        await _context.SupportConversations
            .AddAsync(
                conversation,
                cancellationToken);
    }
}