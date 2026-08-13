using MiniCrm.Application.DTOs.AfterSales;

namespace MiniCrm.Application.Interfaces.Services;

public interface IAfterSalesRequestService
{
    // ============================================================
    // CUSTOMER
    // ============================================================

    Task<AfterSalesRequestDetailResponseDto> CreateAsync(
        Guid userId,
        CreateAfterSalesRequestDto request,
        CancellationToken cancellationToken = default);

    Task<List<AfterSalesRequestSummaryResponseDto>>
        GetMyRequestsAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

    Task<AfterSalesRequestDetailResponseDto>
        GetMyRequestAsync(
            Guid userId,
            int requestId,
            CancellationToken cancellationToken = default);

    Task<AfterSalesRequestDetailResponseDto>
        CancelAsync(
            Guid userId,
            int requestId,
            CancellationToken cancellationToken = default);


    // ============================================================
    // ADMIN
    // ============================================================

    Task<List<AdminAfterSalesRequestSummaryResponseDto>>
        GetAllForAdminAsync(
            CancellationToken cancellationToken = default);

    Task<AdminAfterSalesRequestDetailResponseDto>
        GetAdminRequestAsync(
            int requestId,
            CancellationToken cancellationToken = default);

    Task<AfterSalesRequestDetailResponseDto>
        StartReviewAsync(
            int requestId,
            CancellationToken cancellationToken = default);

    Task<AfterSalesRequestDetailResponseDto>
        ApproveAsync(
            int requestId,
            AfterSalesDecisionRequestDto request,
            CancellationToken cancellationToken = default);

    Task<AfterSalesRequestDetailResponseDto>
        RejectAsync(
            int requestId,
            AfterSalesDecisionRequestDto request,
            CancellationToken cancellationToken = default);

    Task<AfterSalesRequestDetailResponseDto>
        CompleteAsync(
            int requestId,
            CancellationToken cancellationToken = default);
}