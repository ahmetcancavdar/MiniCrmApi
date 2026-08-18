using MiniCrm.Application.DTOs.Categories;
using MiniCrm.Application.Interfaces;
using MiniCrm.Application.Interfaces.Repositories;
using MiniCrm.Application.Interfaces.Services;
using MiniCrm.Domain.Entities;

namespace MiniCrm.Application.Services;

public sealed class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(
        ICategoryRepository categoryRepository,
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<CategoryResponseDto>> GetActiveAsync(
        CancellationToken cancellationToken = default)
    {
        var categories =
            await _categoryRepository.GetAllAsync(
                cancellationToken);

        return categories
            .Where(x => x.IsActive)
            .Select(Map)
            .ToList();
    }

    public async Task<List<CategoryResponseDto>> GetAllForAdminAsync(
        CancellationToken cancellationToken = default)
    {
        var categories =
            await _categoryRepository.GetAllAsync(
                cancellationToken);

        return categories
            .Select(Map)
            .ToList();
    }

    public async Task<CategoryResponseDto> CreateAsync(
        CreateCategoryRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var name = request.Name.Trim();

        var exists =
            await _categoryRepository.ExistsByNameAsync(
                name,
                cancellationToken);

        if (exists)
        {
            throw new InvalidOperationException(
                "A category with this name already exists.");
        }

        var category =
            new Category(
                name,
                request.Description);

        await _categoryRepository.AddAsync(
            category,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Map(category);
    }

    public async Task<CategoryResponseDto> UpdateAsync(
        int id,
        UpdateCategoryRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var category =
            await _categoryRepository.GetByIdAsync(
                id,
                cancellationToken)
            ?? throw new KeyNotFoundException(
                "Category was not found.");

        var categories =
            await _categoryRepository.GetAllAsync(
                cancellationToken);

        var duplicateName =
            categories.Any(x =>
                x.Id != id &&
                string.Equals(
                    x.Name,
                    request.Name.Trim(),
                    StringComparison.OrdinalIgnoreCase));

        if (duplicateName)
        {
            throw new InvalidOperationException(
                "A category with this name already exists.");
        }

        category.ChangeName(
            request.Name);

        category.ChangeDescription(
            request.Description);

        if (request.IsActive)
        {
            category.Activate();
        }
        else
        {
            category.Deactivate();
        }

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Map(category);
    }

    public async Task DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var category =
            await _categoryRepository.GetByIdAsync(
                id,
                cancellationToken)
            ?? throw new KeyNotFoundException(
                "Category was not found.");

        var products =
            await _productRepository.GetByCategoryIdAsync(
                id,
                cancellationToken);

        if (products.Count > 0)
        {
            throw new InvalidOperationException(
                "A category containing products cannot be deleted. Deactivate the category instead.");
        }

        _categoryRepository.Remove(category);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }

    private static CategoryResponseDto Map(
        Category category)
    {
        return new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            IsActive = category.IsActive
        };
    }
}