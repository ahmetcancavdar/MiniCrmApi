using MiniCrm.Application.DTOs.Complaints;

namespace MiniCrm.Application.Interfaces.Services;

public interface IComplaintService
{
    // ============================================================
    // CUSTOMER
    // ============================================================

    Task<ComplaintDetailResponseDto> CreateAsync(
        Guid userId,
        CreateComplaintRequestDto request,
        CancellationToken cancellationToken = default);

    Task<List<ComplaintSummaryResponseDto>>
        GetMyComplaintsAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

    Task<ComplaintDetailResponseDto>
        GetMyComplaintAsync(
            Guid userId,
            int complaintId,
            CancellationToken cancellationToken = default);


    // ============================================================
    // ADMIN
    // ============================================================

    Task<List<AdminComplaintSummaryResponseDto>>
        GetAllForAdminAsync(
            CancellationToken cancellationToken = default);

    Task<AdminComplaintDetailResponseDto>
        GetAdminComplaintAsync(
            int complaintId,
            CancellationToken cancellationToken = default);

    Task<ComplaintDetailResponseDto>
        StartReviewAsync(
            int complaintId,
            CancellationToken cancellationToken = default);

    Task<ComplaintDetailResponseDto>
        ResolveAsync(
            int complaintId,
            ResolveComplaintRequestDto request,
            CancellationToken cancellationToken = default);

    Task<ComplaintDetailResponseDto>
        RejectAsync(
            int complaintId,
            RejectComplaintRequestDto request,
            CancellationToken cancellationToken = default);

    Task<ComplaintDetailResponseDto>
        CloseAsync(
            int complaintId,
            CancellationToken cancellationToken = default);
}