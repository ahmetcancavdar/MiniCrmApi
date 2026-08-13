using Microsoft.EntityFrameworkCore;
using MiniCrm.Application.Interfaces.Repositories;
using MiniCrm.Domain.Entities;
using MiniCrm.Persistence.Context;

namespace MiniCrm.Persistence.Repositories;

public sealed class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Product?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return _context.Products
            .Include(x => x.Category)
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public Task<Product?> GetBySkuAsync(
        string sku,
        CancellationToken cancellationToken = default)
    {
        var normalizedSku = sku
            .Trim()
            .ToUpperInvariant();

        return _context.Products
            .Include(x => x.Category)
            .FirstOrDefaultAsync(
                x => x.SKU == normalizedSku,
                cancellationToken);
    }

    public Task<List<Product>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return _context.Products
            .AsNoTracking()
            .Include(x => x.Category)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<List<Product>> GetByCategoryIdAsync(
        int categoryId,
        CancellationToken cancellationToken = default)
    {
        return _context.Products
            .AsNoTracking()
            .Include(x => x.Category)
            .Where(x => x.CategoryId == categoryId)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<bool> ExistsBySkuAsync(
        string sku,
        CancellationToken cancellationToken = default)
    {
        var normalizedSku = sku
            .Trim()
            .ToUpperInvariant();

        return _context.Products
            .AnyAsync(
                x => x.SKU == normalizedSku,
                cancellationToken);
    }

    public async Task AddAsync(
        Product product,
        CancellationToken cancellationToken = default)
    {
        await _context.Products.AddAsync(
            product,
            cancellationToken);
    }

    public void Remove(Product product)
    {
        _context.Products.Remove(product);
    }
}