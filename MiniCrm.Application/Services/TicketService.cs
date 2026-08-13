using MiniCrm.Application.DTOs.Tickets;
using MiniCrm.Application.Interfaces;
using MiniCrm.Application.Interfaces.Repositories;
using MiniCrm.Application.Interfaces.Services;
using MiniCrm.Domain.Entities;
using MiniCrm.Domain.Enums;

namespace MiniCrm.Application.Services;

public sealed class TicketService
    : ITicketService
{
    private readonly ICustomerRepository
        _customerRepository;

    private readonly ITicketRepository
        _ticketRepository;

    private readonly IOrderRepository
        _orderRepository;

    private readonly IEmailLogRepository
        _emailLogRepository;

    private readonly IUnitOfWork
        _unitOfWork;

    private readonly IEmailService
        _emailService;


    public TicketService(
        ICustomerRepository customerRepository,
        ITicketRepository ticketRepository,
        IOrderRepository orderRepository,
        IEmailLogRepository emailLogRepository,
        IUnitOfWork unitOfWork,
        IEmailService emailService)
    {
        _customerRepository =
            customerRepository;

        _ticketRepository =
            ticketRepository;

        _orderRepository =
            orderRepository;

        _emailLogRepository =
            emailLogRepository;

        _unitOfWork =
            unitOfWork;

        _emailService =
            emailService;
    }


    // ============================================================
    // CUSTOMER - CREATE
    // ============================================================

    public async Task<TicketDetailResponseDto> CreateAsync(
        Guid userId,
        CreateTicketRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var customer =
            await GetCustomerAsync(
                userId,
                cancellationToken);

        if (!Enum.IsDefined(
                typeof(TicketPriority),
                request.Priority))
        {
            throw new InvalidOperationException(
                "Invalid ticket priority.");
        }

        if (request.OrderId.HasValue)
        {
            var order =
                await _orderRepository
                    .GetWithDetailsAsync(
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

        var ticket =
            new Ticket(
                customer.Id,
                request.Subject,
                request.Priority,
                request.OrderId);

        ticket.AddCustomerMessage(
            userId,
            request.Message);

        await _ticketRepository.AddAsync(
            ticket,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        var savedTicket =
            await _ticketRepository
                .GetWithDetailsAsync(
                    ticket.Id,
                    cancellationToken)
            ?? throw new InvalidOperationException(
                "Ticket could not be reloaded.");

        return MapDetail(
            savedTicket);
    }


    // ============================================================
    // CUSTOMER - GET MY TICKETS
    // ============================================================

    public async Task<List<TicketSummaryResponseDto>>
        GetMyTicketsAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
    {
        var customer =
            await GetCustomerAsync(
                userId,
                cancellationToken);

        var tickets =
            await _ticketRepository
                .GetByCustomerIdAsync(
                    customer.Id,
                    cancellationToken);

        return tickets
            .Select(MapSummary)
            .ToList();
    }


    // ============================================================
    // CUSTOMER - GET DETAIL
    // ============================================================

    public async Task<TicketDetailResponseDto>
        GetMyTicketAsync(
            Guid userId,
            int ticketId,
            CancellationToken cancellationToken = default)
    {
        var customer =
            await GetCustomerAsync(
                userId,
                cancellationToken);

        var ticket =
            await GetOwnedTicketAsync(
                customer.Id,
                ticketId,
                cancellationToken);

        return MapDetail(
            ticket);
    }


    // ============================================================
    // CUSTOMER - ADD MESSAGE
    // ============================================================

    public async Task<TicketDetailResponseDto>
        AddCustomerMessageAsync(
            Guid userId,
            int ticketId,
            AddTicketMessageRequestDto request,
            CancellationToken cancellationToken = default)
    {
        var customer =
            await GetCustomerAsync(
                userId,
                cancellationToken);

        var ticket =
            await GetOwnedTicketAsync(
                customer.Id,
                ticketId,
                cancellationToken);

        ticket.AddCustomerMessage(
            userId,
            request.Message);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapDetail(
            ticket);
    }


    // ============================================================
    // ADMIN - GET ALL
    // ============================================================

    public async Task<List<AdminTicketSummaryResponseDto>>
        GetAllForAdminAsync(
            CancellationToken cancellationToken = default)
    {
        var tickets =
            await _ticketRepository.GetAllAsync(
                cancellationToken);

        return tickets
            .Select(ticket =>
                new AdminTicketSummaryResponseDto
                {
                    Id =
                        ticket.Id,

                    CustomerId =
                        ticket.CustomerId,

                    CustomerName =
                        ticket.Customer.FullName,

                    CustomerEmail =
                        ticket.Customer.Email,

                    Subject =
                        ticket.Subject,

                    Priority =
                        ticket.Priority.ToString(),

                    Status =
                        ticket.Status.ToString(),

                    OrderId =
                        ticket.OrderId,

                    CreatedAtUtc =
                        ticket.CreatedAtUtc,

                    UpdatedAtUtc =
                        ticket.UpdatedAtUtc
                })
            .ToList();
    }


    // ============================================================
    // ADMIN - GET DETAIL
    // ============================================================

    public async Task<AdminTicketDetailResponseDto>
        GetAdminTicketAsync(
            int ticketId,
            CancellationToken cancellationToken = default)
    {
        var ticket =
            await GetTicketAsync(
                ticketId,
                cancellationToken);

        return new AdminTicketDetailResponseDto
        {
            CustomerId =
                ticket.CustomerId,

            CustomerName =
                ticket.Customer.FullName,

            CustomerEmail =
                ticket.Customer.Email,

            Ticket =
                MapDetail(ticket)
        };
    }


    // ============================================================
    // ADMIN - ADD MESSAGE
    // ============================================================

    public async Task<TicketDetailResponseDto>
        AddAdminMessageAsync(
            Guid adminUserId,
            int ticketId,
            AddTicketMessageRequestDto request,
            CancellationToken cancellationToken = default)
    {
        if (adminUserId ==
            Guid.Empty)
        {
            throw new UnauthorizedAccessException(
                "Invalid admin identity.");
        }

        var ticket =
            await GetTicketAsync(
                ticketId,
                cancellationToken);

        ticket.AddAdminMessage(
            adminUserId,
            request.Message);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        var subject =
            $"MiniCrm Support Reply - Ticket #{ticket.Id}";

        var body =
            $"""
            Hello {ticket.Customer.FullName},

            Your support ticket has received a new reply.

            Ticket: #{ticket.Id}
            Subject: {ticket.Subject}

            Reply:
            {request.Message.Trim()}

            Current Status: {ticket.Status}
            """;

        await SendTicketEmailAsync(
            ticket,
            subject,
            body,
            cancellationToken);

        return MapDetail(
            ticket);
    }


    // ============================================================
    // ADMIN - START PROGRESS
    // ============================================================

    public async Task<TicketDetailResponseDto>
        StartProgressAsync(
            int ticketId,
            CancellationToken cancellationToken = default)
    {
        var ticket =
            await GetTicketAsync(
                ticketId,
                cancellationToken);

        ticket.StartProgress();

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        var subject =
            $"MiniCrm Ticket Update - Ticket #{ticket.Id}";

        var body =
            $"""
            Hello {ticket.Customer.FullName},

            Your support ticket is now being reviewed.

            Ticket: #{ticket.Id}
            Subject: {ticket.Subject}
            Status: InProgress
            """;

        await SendTicketEmailAsync(
            ticket,
            subject,
            body,
            cancellationToken);

        return MapDetail(
            ticket);
    }


    // ============================================================
    // ADMIN - WAIT FOR CUSTOMER
    // ============================================================

    public async Task<TicketDetailResponseDto>
        SetWaitingForCustomerAsync(
            int ticketId,
            CancellationToken cancellationToken = default)
    {
        var ticket =
            await GetTicketAsync(
                ticketId,
                cancellationToken);

        ticket.WaitForCustomer();

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        var subject =
            $"MiniCrm Ticket Update - Ticket #{ticket.Id}";

        var body =
            $"""
            Hello {ticket.Customer.FullName},

            Your support ticket is waiting for your response.

            Ticket: #{ticket.Id}
            Subject: {ticket.Subject}
            Status: WaitingForCustomer
            """;

        await SendTicketEmailAsync(
            ticket,
            subject,
            body,
            cancellationToken);

        return MapDetail(
            ticket);
    }


    // ============================================================
    // ADMIN - RESOLVE
    // ============================================================

    public async Task<TicketDetailResponseDto>
        ResolveAsync(
            int ticketId,
            CancellationToken cancellationToken = default)
    {
        var ticket =
            await GetTicketAsync(
                ticketId,
                cancellationToken);

        ticket.Resolve(
            DateTime.UtcNow);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        var subject =
            $"MiniCrm Ticket Resolved - Ticket #{ticket.Id}";

        var body =
            $"""
            Hello {ticket.Customer.FullName},

            Your support ticket has been marked as resolved.

            Ticket: #{ticket.Id}
            Subject: {ticket.Subject}
            Status: Resolved

            If you still need help, you can reply to the ticket.
            """;

        await SendTicketEmailAsync(
            ticket,
            subject,
            body,
            cancellationToken);

        return MapDetail(
            ticket);
    }


    // ============================================================
    // ADMIN - CLOSE
    // ============================================================

    public async Task<TicketDetailResponseDto>
        CloseAsync(
            int ticketId,
            CancellationToken cancellationToken = default)
    {
        var ticket =
            await GetTicketAsync(
                ticketId,
                cancellationToken);

        ticket.Close(
            DateTime.UtcNow);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        var subject =
            $"MiniCrm Ticket Closed - Ticket #{ticket.Id}";

        var body =
            $"""
            Hello {ticket.Customer.FullName},

            Your support ticket has been closed.

            Ticket: #{ticket.Id}
            Subject: {ticket.Subject}
            Status: Closed
            """;

        await SendTicketEmailAsync(
            ticket,
            subject,
            body,
            cancellationToken);

        return MapDetail(
            ticket);
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

    private async Task<Ticket> GetOwnedTicketAsync(
        int customerId,
        int ticketId,
        CancellationToken cancellationToken)
    {
        var ticket =
            await _ticketRepository
                .GetWithDetailsAsync(
                    ticketId,
                    cancellationToken)
            ?? throw new KeyNotFoundException(
                "Ticket was not found.");

        if (ticket.CustomerId !=
            customerId)
        {
            throw new UnauthorizedAccessException(
                "You do not have access to this ticket.");
        }

        return ticket;
    }


    // ============================================================
    // ADMIN TICKET
    // ============================================================

    private async Task<Ticket> GetTicketAsync(
        int ticketId,
        CancellationToken cancellationToken)
    {
        return await _ticketRepository
            .GetWithDetailsAsync(
                ticketId,
                cancellationToken)
            ?? throw new KeyNotFoundException(
                "Ticket was not found.");
    }


    // ============================================================
    // EMAIL
    // ============================================================

    private async Task SendTicketEmailAsync(
        Ticket ticket,
        string subject,
        string body,
        CancellationToken cancellationToken)
    {
        var customer =
            ticket.Customer;

        var emailLog =
            new EmailLog(
                customer.Email,
                subject,
                body,
                EmailType.TicketUpdate,
                customer.Id,
                ticketId: ticket.Id);

        await _emailLogRepository.AddAsync(
            emailLog,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        try
        {
            await _emailService.SendAsync(
                customer.Email,
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

    private static TicketSummaryResponseDto MapSummary(
        Ticket ticket)
    {
        return new TicketSummaryResponseDto
        {
            Id =
                ticket.Id,

            Subject =
                ticket.Subject,

            Priority =
                ticket.Priority.ToString(),

            Status =
                ticket.Status.ToString(),

            OrderId =
                ticket.OrderId,

            CreatedAtUtc =
                ticket.CreatedAtUtc,

            UpdatedAtUtc =
                ticket.UpdatedAtUtc,

            ResolvedAtUtc =
                ticket.ResolvedAtUtc,

            ClosedAtUtc =
                ticket.ClosedAtUtc
        };
    }


    // ============================================================
    // DETAIL MAP
    // ============================================================

    private static TicketDetailResponseDto MapDetail(
        Ticket ticket)
    {
        return new TicketDetailResponseDto
        {
            Id =
                ticket.Id,

            Subject =
                ticket.Subject,

            Priority =
                ticket.Priority.ToString(),

            Status =
                ticket.Status.ToString(),

            OrderId =
                ticket.OrderId,

            CreatedAtUtc =
                ticket.CreatedAtUtc,

            UpdatedAtUtc =
                ticket.UpdatedAtUtc,

            ResolvedAtUtc =
                ticket.ResolvedAtUtc,

            ClosedAtUtc =
                ticket.ClosedAtUtc,

            Messages =
                ticket.Messages
                    .OrderBy(x =>
                        x.CreatedAtUtc)
                    .Select(message =>
                        new TicketMessageResponseDto
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