using Microsoft.EntityFrameworkCore;
using MiniCrm.Application.Interfaces.Repositories;
using MiniCrm.Domain.Entities;
using MiniCrm.Domain.Enums;
using MiniCrm.Persistence.Context;

namespace MiniCrm.Persistence.Repositories;

public sealed class AfterSalesRequestRepository
    : IAfterSalesRequestRepository
{
    private readonly AppDbContext
        _context;


    public AfterSalesRequestRepository(
        AppDbContext context)
    {
        _context =
            context;
    }


    // ============================================================
    // GET BY ID
    // ============================================================

    public Task<AfterSalesRequest?> GetByIdAsync(
        int requestId,
        CancellationToken cancellationToken = default)
    {
        return _context.AfterSalesRequests
            .FirstOrDefaultAsync(
                x => x.Id == requestId,
                cancellationToken);
    }


    // ============================================================
    // DETAIL
    // ============================================================

    public Task<AfterSalesRequest?> GetWithDetailsAsync(
        int requestId,
        CancellationToken cancellationToken = default)
    {
        return _context.AfterSalesRequests
            .Include(x =>
                x.Customer)
            .Include(x =>
                x.OrderItem)
                .ThenInclude(x =>
                    x.Order)
            .Include(x =>
                x.OrderItem)
                .ThenInclude(x =>
                    x.Product)
            .FirstOrDefaultAsync(
                x => x.Id == requestId,
                cancellationToken);
    }


    // ============================================================
    // CUSTOMER LIST
    // ============================================================

    public Task<List<AfterSalesRequest>>
        GetByCustomerIdAsync(
            int customerId,
            CancellationToken cancellationToken = default)
    {
        return _context.AfterSalesRequests
            .AsNoTracking()
            .Include(x =>
                x.OrderItem)
                .ThenInclude(x =>
                    x.Order)
            .Include(x =>
                x.OrderItem)
                .ThenInclude(x =>
                    x.Product)
            .Where(x =>
                x.CustomerId == customerId)
            .OrderByDescending(x =>
                x.CreatedAtUtc)
            .ToListAsync(
                cancellationToken);
    }


    // ============================================================
    // ADMIN LIST
    // ============================================================

    public Task<List<AfterSalesRequest>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return _context.AfterSalesRequests
            .AsNoTracking()
            .Include(x =>
                x.Customer)
            .Include(x =>
                x.OrderItem)
                .ThenInclude(x =>
                    x.Order)
            .Include(x =>
                x.OrderItem)
                .ThenInclude(x =>
                    x.Product)
            .OrderByDescending(x =>
                x.CreatedAtUtc)
            .ToListAsync(
                cancellationToken);
    }


    // ============================================================
    // ACTIVE REQUEST CHECK
    // ============================================================

    public Task<bool>
        HasActiveRequestForOrderItemAsync(
            int customerId,
            int orderItemId,
            CancellationToken cancellationToken = default)
    {
        return _context.AfterSalesRequests
            .AnyAsync(
                x =>
                    x.CustomerId ==
                    customerId
                    &&
                    x.OrderItemId ==
                    orderItemId
                    &&
                    (
                        x.Status ==
                        AfterSalesRequestStatus.Requested
                        ||
                        x.Status ==
                        AfterSalesRequestStatus.UnderReview
                        ||
                        x.Status ==
                        AfterSalesRequestStatus.Approved
                    ),
                cancellationToken);
    }


    // ============================================================
    // ADD
    // ============================================================

    public async Task AddAsync(
        AfterSalesRequest request,
        CancellationToken cancellationToken = default)
    {
        await _context.AfterSalesRequests
            .AddAsync(
                request,
                cancellationToken);
    }
}