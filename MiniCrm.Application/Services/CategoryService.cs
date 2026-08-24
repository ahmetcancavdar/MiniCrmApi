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
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(
        ICategoryRepository categoryRepository,
        IProductRepository productRepository,
        ICartRepository cartRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
        _cartRepository = cartRepository;
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

            // Kategori pasife alınınca altındaki ürünler artık satın
            // alınamaz hale gelir (Product.IsPurchasable); müşterilerin
            // sepetinden de aktif olarak kaldırılmaları gerekir, aksi
            // halde ürün silinince/pasife alınınca yaptığımız gibi
            // sepette görünmeye devam ederdi.
            await RemoveCategoryProductsFromAllCartsAsync(
                category.Id,
                cancellationToken);
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

        // Kategorinin ürünleri olsa bile soft-delete edilebilir; ürünler
        // etkilenmez, sadece kategori kendi ürünleriyle birlikte listeden
        // gizlenir (query filter sayesinde). Ama ürünler artık satın
        // alınamaz olduğundan (Product.IsPurchasable, Category.IsDeleted'ı
        // da kontrol eder) müşterilerin sepetinden de kaldırılmaları
        // gerekir.
        await RemoveCategoryProductsFromAllCartsAsync(
            id,
            cancellationToken);

        category.SoftDelete();

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }

    private async Task RemoveCategoryProductsFromAllCartsAsync(
        int categoryId,
        CancellationToken cancellationToken)
    {
        var products =
            await _productRepository.GetByCategoryIdAsync(
                categoryId,
                cancellationToken);

        foreach (var product in products)
        {
            var cartsContainingProduct =
                await _cartRepository.GetAllContainingProductAsync(
                    product.Id,
                    cancellationToken);

            foreach (var cart in cartsContainingProduct)
            {
                cart.RemoveProduct(product.Id);
            }
        }
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