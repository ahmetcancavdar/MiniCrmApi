using Microsoft.EntityFrameworkCore;
using MiniCrm.Application.Interfaces;
using MiniCrm.Persistence.Context;

namespace MiniCrm.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(
        AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context
                .SaveChangesAsync(
                    cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new InvalidOperationException(
                "The data was changed by another operation. Please try again.");
        }
    }
}