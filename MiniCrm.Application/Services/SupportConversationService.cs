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

    private readonly IOrderRepository
        _orderRepository;

    private readonly IEmailLogRepository
        _emailLogRepository;

    private readonly IEmailService
        _emailService;

    private readonly IAdminDirectoryService
        _adminDirectoryService;

    private readonly IRealtimeNotifier
        _realtimeNotifier;

    private readonly IUnitOfWork
        _unitOfWork;


    public SupportConversationService(
        ICustomerRepository customerRepository,
        ISupportConversationRepository conversationRepository,
        IOrderRepository orderRepository,
        IEmailLogRepository emailLogRepository,
        IEmailService emailService,
        IAdminDirectoryService adminDirectoryService,
        IRealtimeNotifier realtimeNotifier,
        IUnitOfWork unitOfWork)
    {
        _customerRepository =
            customerRepository;

        _conversationRepository =
            conversationRepository;

        _orderRepository =
            orderRepository;

        _emailLogRepository =
            emailLogRepository;

        _emailService =
            emailService;

        _adminDirectoryService =
            adminDirectoryService;

        _realtimeNotifier =
            realtimeNotifier;

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


        // ========================================================
        // OPTIONAL ORDER OWNERSHIP
        // ========================================================

        if (request.OrderId.HasValue)
        {
            var order =
                await _orderRepository
                    .GetByIdAsync(
                        request.OrderId.Value,
                        cancellationToken)
                ?? throw new KeyNotFoundException(
                    "Order was not found.");

            if (order.CustomerId !=
                customer.Id)
            {
                throw new UnauthorizedAccessException(
                    "You do not have access to this order.");
            }
        }

        var conversation =
            new SupportConversation(
                customer.Id,
                request.OrderId);

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

        await NotifyAdminsOfCustomerMessageAsync(
            savedConversation,
            request.Message,
            cancellationToken);

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

        await NotifyAdminsOfCustomerMessageAsync(
            conversation,
            request.Message,
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

                    OrderId =
                        conversation.OrderId,

                    OrderNumber =
                        conversation.Order?.OrderNumber,

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

        await NotifyCustomerOfAdminMessageAsync(
            conversation,
            request.Message,
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

        try
        {
            await _realtimeNotifier.NotifyCustomerAsync(
                conversation.Customer.UserId,
                "SupportConversationClosed",
                conversation.Id,
                cancellationToken);
        }
        catch
        {
            // Best-effort; kapatma işleminin kendisi zaten tamamlandı.
        }

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
    // NOTIFY ADMINS
    // ============================================================

    private async Task NotifyAdminsOfCustomerMessageAsync(
        SupportConversation conversation,
        string message,
        CancellationToken cancellationToken)
    {
        // Gerçek zamanlı bildirim best-effort'tur: başarısız olsa bile
        // mesaj gönderme işleminin kendisi asla etkilenmemeli (e-posta
        // gönderiminde uygulanan aynı felsefe).
        try
        {
            await _realtimeNotifier.NotifyAdminsAsync(
                "SupportMessageReceived",
                conversation.Id,
                cancellationToken);
        }
        catch
        {
            // Bağlı admin olmaması ya da geçici bir SignalR hatası
            // ana akışı bozmamalı.
        }

        var adminEmails =
            await _adminDirectoryService.GetAdminEmailsAsync(
                cancellationToken);

        var subject =
            $"MiniCrm Support - New Message in Conversation #{conversation.Id}";

        var body =
            $"""
            Hello,

            {conversation.Customer.FullName} ({conversation.Customer.Email}) sent a new message in support conversation #{conversation.Id}.

            Message:
            {message}
            """;

        foreach (var adminEmail in adminEmails)
        {
            await SendSupportEmailAsync(
                adminEmail,
                conversation.CustomerId,
                subject,
                body,
                cancellationToken);
        }
    }


    // ============================================================
    // NOTIFY CUSTOMER
    // ============================================================

    private async Task NotifyCustomerOfAdminMessageAsync(
        SupportConversation conversation,
        string message,
        CancellationToken cancellationToken)
    {
        try
        {
            await _realtimeNotifier.NotifyCustomerAsync(
                conversation.Customer.UserId,
                "SupportMessageReceived",
                conversation.Id,
                cancellationToken);
        }
        catch
        {
            // Best-effort; müşteri o an bağlı değilse ya da geçici bir
            // SignalR hatası oluşursa ana akış etkilenmemeli.
        }

        var subject =
            $"MiniCrm Support - New Reply in Conversation #{conversation.Id}";

        var body =
            $"""
            Hello {conversation.Customer.FullName},

            You have a new reply in support conversation #{conversation.Id}.

            Message:
            {message}
            """;

        await SendSupportEmailAsync(
            conversation.Customer.Email,
            conversation.CustomerId,
            subject,
            body,
            cancellationToken);
    }


    // ============================================================
    // EMAIL
    // ============================================================

    private async Task SendSupportEmailAsync(
        string toEmail,
        int customerId,
        string subject,
        string body,
        CancellationToken cancellationToken)
    {
        var emailLog =
            new EmailLog(
                toEmail,
                subject,
                body,
                EmailType.SupportMessage,
                customerId);

        await _emailLogRepository.AddAsync(
            emailLog,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        try
        {
            await _emailService.SendAsync(
                toEmail,
                subject,
                body,
                cancellationToken);

            emailLog.MarkAsSent(
                DateTime.UtcNow);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);
        }
        catch (Exception exception)
        {
            emailLog.MarkAsFailed(
                exception.Message,
                DateTime.UtcNow);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);
        }
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

            OrderId =
                conversation.OrderId,

            OrderNumber =
                conversation.Order?.OrderNumber,

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

            OrderId =
                conversation.OrderId,

            OrderNumber =
                conversation.Order?.OrderNumber,

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