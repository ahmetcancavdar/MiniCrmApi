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
        return QueryProductsWithCategory()
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

        return QueryProductsWithCategory()
            .FirstOrDefaultAsync(
                x => x.SKU == normalizedSku,
                cancellationToken);
    }

    public Task<List<Product>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return QueryProductsWithCategory()
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<List<Product>> GetByCategoryIdAsync(
        int categoryId,
        CancellationToken cancellationToken = default)
    {
        return QueryProductsWithCategory()
            .AsNoTracking()
            .Where(x => x.CategoryId == categoryId)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    // Kategori soft-delete edilmiş olsa bile o kategorideki ürünler
    // görünmeye devam etmeli. EF Core, Include edilen bir ilişkide
    // karşı tarafın (Category) global soft-delete filtresini de
    // uyguluyor; FK zorunlu olduğu için bu durumda ürün satırı da
    // JOIN'den düşüyor. Bunu önlemek için filtreler devre dışı
    // bırakılıp sadece ürünün kendi IsDeleted durumu elle kontrol
    // ediliyor; Category'nin silinmiş olması ürünü etkilemiyor.
    private IQueryable<Product> QueryProductsWithCategory()
    {
        return _context.Products
            .IgnoreQueryFilters()
            .Where(x => !x.IsDeleted)
            .Include(x => x.Category);
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
}