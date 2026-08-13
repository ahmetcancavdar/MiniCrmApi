using MiniCrm.Application.DTOs.Categories;

namespace MiniCrm.Application.Interfaces.Services;

public interface ICategoryService
{
    Task<List<CategoryResponseDto>> GetActiveAsync(
        CancellationToken cancellationToken = default);

    Task<List<CategoryResponseDto>> GetAllForAdminAsync(
        CancellationToken cancellationToken = default);

    Task<CategoryResponseDto> CreateAsync(
        CreateCategoryRequestDto request,
        CancellationToken cancellationToken = default);

    Task<CategoryResponseDto> UpdateAsync(
        int id,
        UpdateCategoryRequestDto request,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        int id,
        CancellationToken cancellationToken = default);
}