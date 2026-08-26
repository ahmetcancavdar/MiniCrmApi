using MiniCrm.Application.DTOs.Leads;

namespace MiniCrm.Application.Interfaces.Services;

public interface ILeadService
{
    Task<List<LeadResponseDto>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<LeadDetailResponseDto> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<LeadResponseDto> CreateAsync(
        CreateLeadRequestDto request,
        CancellationToken cancellationToken = default);

    Task<LeadResponseDto> UpdateAsync(
        int id,
        UpdateLeadRequestDto request,
        CancellationToken cancellationToken = default);

    Task<LeadResponseDto> UpdateStatusAsync(
        int id,
        Guid adminUserId,
        UpdateLeadStatusRequestDto request,
        CancellationToken cancellationToken = default);

    Task<LeadDetailResponseDto> AddNoteAsync(
        int id,
        Guid adminUserId,
        AddLeadNoteRequestDto request,
        CancellationToken cancellationToken = default);

    Task<LeadResponseDto> ConvertToCustomerAsync(
        int id,
        ConvertLeadRequestDto request,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        int id,
        CancellationToken cancellationToken = default);
}
