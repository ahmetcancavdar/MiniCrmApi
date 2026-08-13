using MiniCrm.Application.DTOs.Tickets;

namespace MiniCrm.Application.Interfaces.Services;

public interface ITicketService
{
    // ============================================================
    // CUSTOMER
    // ============================================================

    Task<TicketDetailResponseDto> CreateAsync(
        Guid userId,
        CreateTicketRequestDto request,
        CancellationToken cancellationToken = default);

    Task<List<TicketSummaryResponseDto>> GetMyTicketsAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<TicketDetailResponseDto> GetMyTicketAsync(
        Guid userId,
        int ticketId,
        CancellationToken cancellationToken = default);

    Task<TicketDetailResponseDto> AddCustomerMessageAsync(
        Guid userId,
        int ticketId,
        AddTicketMessageRequestDto request,
        CancellationToken cancellationToken = default);


    // ============================================================
    // ADMIN
    // ============================================================

    Task<List<AdminTicketSummaryResponseDto>>
        GetAllForAdminAsync(
            CancellationToken cancellationToken = default);

    Task<AdminTicketDetailResponseDto>
        GetAdminTicketAsync(
            int ticketId,
            CancellationToken cancellationToken = default);

    Task<TicketDetailResponseDto> AddAdminMessageAsync(
        Guid adminUserId,
        int ticketId,
        AddTicketMessageRequestDto request,
        CancellationToken cancellationToken = default);

    Task<TicketDetailResponseDto> StartProgressAsync(
        int ticketId,
        CancellationToken cancellationToken = default);

    Task<TicketDetailResponseDto> SetWaitingForCustomerAsync(
        int ticketId,
        CancellationToken cancellationToken = default);

    Task<TicketDetailResponseDto> ResolveAsync(
        int ticketId,
        CancellationToken cancellationToken = default);

    Task<TicketDetailResponseDto> CloseAsync(
        int ticketId,
        CancellationToken cancellationToken = default);
}