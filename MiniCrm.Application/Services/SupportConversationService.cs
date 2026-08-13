using MiniCrm.Application.DTOs.Support;
using MiniCrm.Application.Interfaces;
using MiniCrm.Application.Interfaces.Repositories;
using MiniCrm.Application.Interfaces.Services;
using MiniCrm.Domain.Entities;
using MiniCrm.Domain.Enums;

namespace MiniCrm.Application.Services;

public sealed class SupportConversationService
    : ISupportConversationService
{
    private readonly ICustomerRepository
        _customerRepository;

    private readonly ISupportConversationRepository
        _conversationRepository;

    private readonly IUnitOfWork
        _unitOfWork;


    public SupportConversationService(
        ICustomerRepository customerRepository,
        ISupportConversationRepository conversationRepository,
        IUnitOfWork unitOfWork)
    {
        _customerRepository =
            customerRepository;

        _conversationRepository =
            conversationRepository;

        _unitOfWork =
            unitOfWork;
    }


    // ============================================================
    // CUSTOMER - CREATE
    // ============================================================

    public async Task<SupportConversationDetailResponseDto>
        CreateAsync(
            Guid userId,
            CreateSupportConversationRequestDto request,
            CancellationToken cancellationToken = default)
    {
        var customer =
            await GetCustomerAsync(
                userId,
                cancellationToken);

        var existingConversations =
            await _conversationRepository
                .GetByCustomerIdAsync(
                    customer.Id,
                    cancellationToken);

        var hasOpenConversation =
            existingConversations.Any(x =>
                x.Status ==
                SupportConversationStatus.Open);

        if (hasOpenConversation)
        {
            throw new InvalidOperationException(
                "You already have an open support conversation.");
        }

        var conversation =
            new SupportConversation(
                customer.Id);

        conversation.AddCustomerMessage(
            userId,
            request.Message);

        await _conversationRepository.AddAsync(
            conversation,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        var savedConversation =
            await _conversationRepository
                .GetWithDetailsAsync(
                    conversation.Id,
                    cancellationToken)
            ?? throw new InvalidOperationException(
                "Support conversation could not be reloaded.");

        return MapDetail(
            savedConversation);
    }


    // ============================================================
    // CUSTOMER - LIST
    // ============================================================

    public async Task<List<SupportConversationSummaryResponseDto>>
        GetMyConversationsAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
    {
        var customer =
            await GetCustomerAsync(
                userId,
                cancellationToken);

        var conversations =
            await _conversationRepository
                .GetByCustomerIdAsync(
                    customer.Id,
                    cancellationToken);

        return conversations
            .Select(MapSummary)
            .ToList();
    }


    // ============================================================
    // CUSTOMER - DETAIL
    // ============================================================

    public async Task<SupportConversationDetailResponseDto>
        GetMyConversationAsync(
            Guid userId,
            int conversationId,
            CancellationToken cancellationToken = default)
    {
        var customer =
            await GetCustomerAsync(
                userId,
                cancellationToken);

        var conversation =
            await GetOwnedConversationAsync(
                customer.Id,
                conversationId,
                cancellationToken);

        return MapDetail(
            conversation);
    }


    // ============================================================
    // CUSTOMER - MESSAGE
    // ============================================================

    public async Task<SupportConversationDetailResponseDto>
        AddCustomerMessageAsync(
            Guid userId,
            int conversationId,
            AddSupportMessageRequestDto request,
            CancellationToken cancellationToken = default)
    {
        var customer =
            await GetCustomerAsync(
                userId,
                cancellationToken);

        var conversation =
            await GetOwnedConversationAsync(
                customer.Id,
                conversationId,
                cancellationToken);

        conversation.AddCustomerMessage(
            userId,
            request.Message);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapDetail(
            conversation);
    }


    // ============================================================
    // ADMIN - LIST
    // ============================================================

    public async Task<List<AdminSupportConversationSummaryResponseDto>>
        GetAllForAdminAsync(
            CancellationToken cancellationToken = default)
    {
        var conversations =
            await _conversationRepository.GetAllAsync(
                cancellationToken);

        return conversations
            .Select(conversation =>
                new AdminSupportConversationSummaryResponseDto
                {
                    Id =
                        conversation.Id,

                    CustomerId =
                        conversation.CustomerId,

                    CustomerName =
                        conversation.Customer.FullName,

                    CustomerEmail =
                        conversation.Customer.Email,

                    Status =
                        conversation.Status.ToString(),

                    CreatedAtUtc =
                        conversation.CreatedAtUtc,

                    UpdatedAtUtc =
                        conversation.UpdatedAtUtc
                })
            .ToList();
    }


    // ============================================================
    // ADMIN - DETAIL
    // ============================================================

    public async Task<AdminSupportConversationDetailResponseDto>
        GetAdminConversationAsync(
            int conversationId,
            CancellationToken cancellationToken = default)
    {
        var conversation =
            await GetConversationAsync(
                conversationId,
                cancellationToken);

        return new AdminSupportConversationDetailResponseDto
        {
            CustomerId =
                conversation.CustomerId,

            CustomerName =
                conversation.Customer.FullName,

            CustomerEmail =
                conversation.Customer.Email,

            Conversation =
                MapDetail(
                    conversation)
        };
    }


    // ============================================================
    // ADMIN - MESSAGE
    // ============================================================

    public async Task<SupportConversationDetailResponseDto>
        AddAdminMessageAsync(
            Guid adminUserId,
            int conversationId,
            AddSupportMessageRequestDto request,
            CancellationToken cancellationToken = default)
    {
        if (adminUserId ==
            Guid.Empty)
        {
            throw new UnauthorizedAccessException(
                "Invalid admin identity.");
        }

        var conversation =
            await GetConversationAsync(
                conversationId,
                cancellationToken);

        conversation.AddAdminMessage(
            adminUserId,
            request.Message);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapDetail(
            conversation);
    }


    // ============================================================
    // ADMIN - CLOSE
    // ============================================================

    public async Task<SupportConversationDetailResponseDto>
        CloseAsync(
            int conversationId,
            CancellationToken cancellationToken = default)
    {
        var conversation =
            await GetConversationAsync(
                conversationId,
                cancellationToken);

        conversation.Close();

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapDetail(
            conversation);
    }


    // ============================================================
    // CUSTOMER
    // ============================================================

    private async Task<Customer> GetCustomerAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        if (userId ==
            Guid.Empty)
        {
            throw new UnauthorizedAccessException(
                "Invalid user.");
        }

        return await _customerRepository
            .GetByUserIdAsync(
                userId,
                cancellationToken)
            ?? throw new UnauthorizedAccessException(
                "Customer profile was not found.");
    }


    // ============================================================
    // OWNERSHIP
    // ============================================================

    private async Task<SupportConversation>
        GetOwnedConversationAsync(
            int customerId,
            int conversationId,
            CancellationToken cancellationToken)
    {
        var conversation =
            await _conversationRepository
                .GetWithDetailsAsync(
                    conversationId,
                    cancellationToken)
            ?? throw new KeyNotFoundException(
                "Support conversation was not found.");

        if (conversation.CustomerId !=
            customerId)
        {
            throw new UnauthorizedAccessException(
                "You do not have access to this support conversation.");
        }

        return conversation;
    }


    // ============================================================
    // ADMIN GET
    // ============================================================

    private async Task<SupportConversation>
        GetConversationAsync(
            int conversationId,
            CancellationToken cancellationToken)
    {
        return await _conversationRepository
            .GetWithDetailsAsync(
                conversationId,
                cancellationToken)
            ?? throw new KeyNotFoundException(
                "Support conversation was not found.");
    }


    // ============================================================
    // SUMMARY MAP
    // ============================================================

    private static SupportConversationSummaryResponseDto
        MapSummary(
            SupportConversation conversation)
    {
        return new SupportConversationSummaryResponseDto
        {
            Id =
                conversation.Id,

            Status =
                conversation.Status.ToString(),

            CreatedAtUtc =
                conversation.CreatedAtUtc,

            UpdatedAtUtc =
                conversation.UpdatedAtUtc
        };
    }


    // ============================================================
    // DETAIL MAP
    // ============================================================

    private static SupportConversationDetailResponseDto
        MapDetail(
            SupportConversation conversation)
    {
        return new SupportConversationDetailResponseDto
        {
            Id =
                conversation.Id,

            Status =
                conversation.Status.ToString(),

            CreatedAtUtc =
                conversation.CreatedAtUtc,

            UpdatedAtUtc =
                conversation.UpdatedAtUtc,

            Messages =
                conversation.Messages
                    .OrderBy(x =>
                        x.CreatedAtUtc)
                    .ThenBy(x =>
                        x.Id)
                    .Select(message =>
                        new SupportMessageResponseDto
                        {
                            Id =
                                message.Id,

                            SenderUserId =
                                message.SenderUserId,

                            SenderType =
                                message.SenderType.ToString(),

                            Message =
                                message.Message,

                            CreatedAtUtc =
                                message.CreatedAtUtc
                        })
                    .ToList()
        };
    }
}