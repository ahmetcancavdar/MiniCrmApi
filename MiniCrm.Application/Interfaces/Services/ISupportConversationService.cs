using MiniCrm.Application.DTOs.Support;

namespace MiniCrm.Application.Interfaces.Services;

public interface ISupportConversationService
{
    // ============================================================
    // CUSTOMER
    // ============================================================

    Task<SupportConversationDetailResponseDto> CreateAsync(
        Guid userId,
        CreateSupportConversationRequestDto request,
        CancellationToken cancellationToken = default);

    Task<List<SupportConversationSummaryResponseDto>>
        GetMyConversationsAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

    Task<SupportConversationDetailResponseDto>
        GetMyConversationAsync(
            Guid userId,
            int conversationId,
            CancellationToken cancellationToken = default);

    Task<SupportConversationDetailResponseDto>
        AddCustomerMessageAsync(
            Guid userId,
            int conversationId,
            AddSupportMessageRequestDto request,
            CancellationToken cancellationToken = default);


    // ============================================================
    // ADMIN
    // ============================================================

    Task<List<AdminSupportConversationSummaryResponseDto>>
        GetAllForAdminAsync(
            CancellationToken cancellationToken = default);

    Task<AdminSupportConversationDetailResponseDto>
        GetAdminConversationAsync(
            int conversationId,
            CancellationToken cancellationToken = default);

    Task<SupportConversationDetailResponseDto>
        AddAdminMessageAsync(
            Guid adminUserId,
            int conversationId,
            AddSupportMessageRequestDto request,
            CancellationToken cancellationToken = default);

    Task<SupportConversationDetailResponseDto>
        CloseAsync(
            int conversationId,
            CancellationToken cancellationToken = default);
}