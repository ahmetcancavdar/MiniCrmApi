using MiniCrm.Domain.Entities;

namespace MiniCrm.Application.Interfaces.Repositories;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<Product?> GetBySkuAsync(
        string sku,
        CancellationToken cancellationToken = default);

    Task<List<Product>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<List<Product>> GetByCategoryIdAsync(
        int categoryId,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsBySkuAsync(
        string sku,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Product product,
        CancellationToken cancellationToken = default);

    void Remove(Product product);
}