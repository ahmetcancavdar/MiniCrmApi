using MiniCrm.Application.DTOs.Cart;

namespace MiniCrm.Application.Interfaces.Services;

public interface ICartService
{
    Task<CartResponseDto> GetAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<CartResponseDto> AddItemAsync(
        Guid userId,
        AddCartItemRequestDto request,
        CancellationToken cancellationToken = default);

    Task<CartResponseDto> UpdateQuantityAsync(
        Guid userId,
        int productId,
        UpdateCartItemQuantityRequestDto request,
        CancellationToken cancellationToken = default);

    Task<CartResponseDto> RemoveItemAsync(
        Guid userId,
        int productId,
        CancellationToken cancellationToken = default);

    Task<CartResponseDto> ClearAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
}