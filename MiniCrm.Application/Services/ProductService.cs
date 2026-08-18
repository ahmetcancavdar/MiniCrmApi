using MiniCrm.Application.DTOs.Products;
using MiniCrm.Application.Interfaces;
using MiniCrm.Application.Interfaces.Repositories;
using MiniCrm.Application.Interfaces.Services;
using MiniCrm.Domain.Entities;
using MiniCrm.Domain.Enums;

namespace MiniCrm.Application.Services;

public sealed class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IStockMovementRepository _stockMovementRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IStockMovementRepository stockMovementRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _stockMovementRepository = stockMovementRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<ProductResponseDto>> GetActiveAsync(
        CancellationToken cancellationToken = default)
    {
        var products =
            await _productRepository.GetAllAsync(
                cancellationToken);

        return products
            .Where(x =>
                x.IsActive &&
                x.Category.IsActive)
            .Select(Map)
            .ToList();
    }

    public async Task<ProductResponseDto> GetActiveByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var product =
            await _productRepository.GetByIdAsync(
                id,
                cancellationToken)
            ?? throw new KeyNotFoundException(
                "Product was not found.");

        if (!product.IsActive ||
            !product.Category.IsActive)
        {
            throw new KeyNotFoundException(
                "Product was not found.");
        }

        return Map(product);
    }

    public async Task<List<ProductResponseDto>> GetAllForAdminAsync(
        CancellationToken cancellationToken = default)
    {
        var products =
            await _productRepository.GetAllAsync(
                cancellationToken);

        return products
            .Select(Map)
            .ToList();
    }

    public async Task<ProductResponseDto> CreateAsync(
        CreateProductRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var category =
            await _categoryRepository.GetByIdAsync(
                request.CategoryId,
                cancellationToken)
            ?? throw new InvalidOperationException(
                "Selected category does not exist.");

        var skuExists =
            await _productRepository.ExistsBySkuAsync(
                request.SKU,
                cancellationToken);

        if (skuExists)
        {
            throw new InvalidOperationException(
                "A product with this SKU already exists.");
        }

        var product =
            new Product(
                category.Id,
                request.Name,
                request.SKU,
                request.Description,
                request.Price,
                request.ImageUrl);

        await _productRepository.AddAsync(
            product,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Map(product, category.Name);
    }

    public async Task<ProductResponseDto> UpdateAsync(
        int id,
        UpdateProductRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var product =
            await _productRepository.GetByIdAsync(
                id,
                cancellationToken)
            ?? throw new KeyNotFoundException(
                "Product was not found.");

        var category =
            await _categoryRepository.GetByIdAsync(
                request.CategoryId,
                cancellationToken)
            ?? throw new InvalidOperationException(
                "Selected category does not exist.");

        var productWithSku =
            await _productRepository.GetBySkuAsync(
                request.SKU,
                cancellationToken);

        if (productWithSku is not null &&
            productWithSku.Id != product.Id)
        {
            throw new InvalidOperationException(
                "A product with this SKU already exists.");
        }

        product.ChangeCategory(
            request.CategoryId);

        product.ChangeName(
            request.Name);

        product.ChangeSku(
            request.SKU);

        product.ChangeDescription(
            request.Description);

        product.ChangePrice(
            request.Price);

        product.ChangeImageUrl(
            request.ImageUrl);

        if (request.IsActive)
        {
            product.Activate();
        }
        else
        {
            product.Deactivate();
        }

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Map(product, category.Name);
    }

    public async Task DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var product =
            await _productRepository.GetByIdAsync(
                id,
                cancellationToken)
            ?? throw new KeyNotFoundException(
                "Product was not found.");

        _productRepository.Remove(product);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }

    public async Task<ProductResponseDto> AdjustStockAsync(
        int id,
        AdjustStockRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var product =
            await _productRepository.GetByIdAsync(
                id,
                cancellationToken)
            ?? throw new KeyNotFoundException(
                "Product was not found.");

        if (request.QuantityChange == 0)
        {
            throw new InvalidOperationException(
                "Stock quantity change cannot be zero.");
        }

        var previousQuantity =
            product.StockQuantity;

        StockMovementType movementType;

        if (request.QuantityChange > 0)
        {
            product.IncreaseStock(
                request.QuantityChange);

            movementType =
                StockMovementType.AdminIncrease;
        }
        else
        {
            var quantityToDecrease =
                Math.Abs(request.QuantityChange);

            product.DecreaseStock(
                quantityToDecrease);

            movementType =
                StockMovementType.AdminDecrease;
        }

        var stockMovement =
            new StockMovement(
                product.Id,
                movementType,
                request.QuantityChange,
                previousQuantity,
                product.StockQuantity,
                request.Note);

        await _stockMovementRepository.AddAsync(
            stockMovement,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Map(product);
    }

    public async Task<List<StockMovementResponseDto>>
        GetStockMovementsAsync(
            int productId,
            CancellationToken cancellationToken = default)
    {
        var product =
            await _productRepository.GetByIdAsync(
                productId,
                cancellationToken)
            ?? throw new KeyNotFoundException(
                "Product was not found.");

        var movements =
            await _stockMovementRepository
                .GetByProductIdAsync(
                    product.Id,
                    cancellationToken);

        return movements
            .Select(x =>
                new StockMovementResponseDto
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    MovementType =
                        x.MovementType.ToString(),
                    QuantityChange =
                        x.QuantityChange,
                    PreviousQuantity =
                        x.PreviousQuantity,
                    NewQuantity =
                        x.NewQuantity,
                    Note = x.Note,
                    CreatedAtUtc =
                        x.CreatedAtUtc
                })
            .ToList();
    }

    private static ProductResponseDto Map(
        Product product)
    {
        return Map(
            product,
            product.Category?.Name
            ?? string.Empty);
    }

    private static ProductResponseDto Map(
        Product product,
        string categoryName)
    {
        return new ProductResponseDto
        {
            Id = product.Id,
            CategoryId = product.CategoryId,
            CategoryName = categoryName,
            Name = product.Name,
            SKU = product.SKU,
            Description = product.Description,
            Price = product.Price,
            StockQuantity =
                product.StockQuantity,
            ImageUrl = product.ImageUrl,
            IsActive = product.IsActive
        };
    }
}