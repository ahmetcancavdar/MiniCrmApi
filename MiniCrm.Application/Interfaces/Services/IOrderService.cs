using MiniCrm.Application.DTOs.Orders;

namespace MiniCrm.Application.Interfaces.Services;

public interface IOrderService
{
    // ============================================================
    // CUSTOMER
    // ============================================================

    Task<CheckoutOrderResponseDto> CheckoutAsync(
        Guid userId,
        CheckoutOrderRequestDto request,
        CancellationToken cancellationToken = default);

    Task<OrderResponseDto> VerifyAsync(
        Guid userId,
        int orderId,
        VerifyOrderRequestDto request,
        CancellationToken cancellationToken = default);

    Task<VerificationEmailResponseDto>
        ResendVerificationCodeAsync(
            Guid userId,
            int orderId,
            CancellationToken cancellationToken = default);

    Task<OrderResponseDto> GetByIdAsync(
        Guid userId,
        int orderId,
        CancellationToken cancellationToken = default);

    Task<List<OrderSummaryResponseDto>> GetMyOrdersAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<OrderResponseDto> CancelAsync(
        Guid userId,
        int orderId,
        CancelOrderRequestDto request,
        CancellationToken cancellationToken = default);


    // ============================================================
    // ADMIN
    // ============================================================

    Task<List<AdminOrderSummaryResponseDto>>
        GetAllForAdminAsync(
            CancellationToken cancellationToken = default);

    Task<AdminOrderDetailResponseDto>
        GetAdminByIdAsync(
            int orderId,
            CancellationToken cancellationToken = default);

    Task<OrderResponseDto> StartPreparingAsync(
        int orderId,
        CancellationToken cancellationToken = default);

    Task<OrderResponseDto> MarkAsShippedAsync(
        int orderId,
        CancellationToken cancellationToken = default);

    Task<OrderResponseDto> MarkAsDeliveredAsync(
        int orderId,
        CancellationToken cancellationToken = default);

    Task<OrderResponseDto> CancelByAdminAsync(
        int orderId,
        CancelOrderRequestDto request,
        CancellationToken cancellationToken = default);
}