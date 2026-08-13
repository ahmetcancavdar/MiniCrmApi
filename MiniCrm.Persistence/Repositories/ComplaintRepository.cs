using Microsoft.EntityFrameworkCore;
using MiniCrm.Application.Interfaces.Repositories;
using MiniCrm.Domain.Entities;
using MiniCrm.Persistence.Context;

namespace MiniCrm.Persistence.Repositories;

public sealed class ComplaintRepository
    : IComplaintRepository
{
    private readonly AppDbContext
        _context;


    public ComplaintRepository(
        AppDbContext context)
    {
        _context =
            context;
    }


    // ============================================================
    // GET BY ID
    // ============================================================

    public Task<Complaint?> GetByIdAsync(
        int complaintId,
        CancellationToken cancellationToken = default)
    {
        return _context.Complaints
            .FirstOrDefaultAsync(
                x => x.Id == complaintId,
                cancellationToken);
    }


    // ============================================================
    // GET DETAILS
    // ============================================================

    public Task<Complaint?> GetWithDetailsAsync(
        int complaintId,
        CancellationToken cancellationToken = default)
    {
        return _context.Complaints
            .Include(x => x.Customer)
            .Include(x => x.Order)
            .FirstOrDefaultAsync(
                x => x.Id == complaintId,
                cancellationToken);
    }


    // ============================================================
    // CUSTOMER LIST
    // ============================================================

    public Task<List<Complaint>>
        GetByCustomerIdAsync(
            int customerId,
            CancellationToken cancellationToken = default)
    {
        return _context.Complaints
            .AsNoTracking()
            .Include(x => x.Order)
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

    public Task<List<Complaint>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return _context.Complaints
            .AsNoTracking()
            .Include(x => x.Customer)
            .Include(x => x.Order)
            .OrderByDescending(x =>
                x.CreatedAtUtc)
            .ToListAsync(
                cancellationToken);
    }


    // ============================================================
    // ADD
    // ============================================================

    public async Task AddAsync(
        Complaint complaint,
        CancellationToken cancellationToken = default)
    {
        await _context.Complaints.AddAsync(
            complaint,
            cancellationToken);
    }
}