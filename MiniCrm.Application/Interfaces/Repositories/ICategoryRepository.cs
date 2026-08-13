using MiniCrm.Domain.Entities;

namespace MiniCrm.Application.Interfaces.Repositories;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<List<Category>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByNameAsync(
        string name,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Category category,
        CancellationToken cancellationToken = default);

    void Remove(Category category);
}