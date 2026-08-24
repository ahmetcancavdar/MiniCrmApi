using Microsoft.EntityFrameworkCore;
using MiniCrm.Application.Interfaces.Repositories;
using MiniCrm.Domain.Entities;
using MiniCrm.Persistence.Context;

namespace MiniCrm.Persistence.Repositories;

public sealed class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Category?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return _context.Categories
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public Task<List<Category>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return _context.Categories
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<bool> ExistsByNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        var normalizedName = name.Trim();

        return _context.Categories
            .AnyAsync(
                x => x.Name == normalizedName,
                cancellationToken);
    }

    public async Task AddAsync(
        Category category,
        CancellationToken cancellationToken = default)
    {
        await _context.Categories.AddAsync(
            category,
            cancellationToken);
    }
}