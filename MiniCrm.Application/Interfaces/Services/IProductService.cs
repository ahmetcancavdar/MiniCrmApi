using MiniCrm.Application.DTOs.Products;

namespace MiniCrm.Application.Interfaces.Services;

public interface IProductService
{
    Task<List<ProductResponseDto>> GetActiveAsync(
        CancellationToken cancellationToken = default);

    Task<ProductResponseDto> GetActiveByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<List<ProductResponseDto>> GetAllForAdminAsync(
        CancellationToken cancellationToken = default);

    Task<ProductResponseDto> CreateAsync(
        CreateProductRequestDto request,
        CancellationToken cancellationToken = default);

    Task<ProductResponseDto> UpdateAsync(
        int id,
        UpdateProductRequestDto request,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<ProductResponseDto> AdjustStockAsync(
        int id,
        AdjustStockRequestDto request,
        CancellationToken cancellationToken = default);

    Task<List<StockMovementResponseDto>> GetStockMovementsAsync(
        int productId,
        CancellationToken cancellationToken = default);
}