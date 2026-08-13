using MiniCrm.Application.DTOs.Complaints;
using MiniCrm.Application.Interfaces;
using MiniCrm.Application.Interfaces.Repositories;
using MiniCrm.Application.Interfaces.Services;
using MiniCrm.Domain.Entities;
using MiniCrm.Domain.Enums;

namespace MiniCrm.Application.Services;

public sealed class ComplaintService
    : IComplaintService
{
    private readonly ICustomerRepository
        _customerRepository;

    private readonly IComplaintRepository
        _complaintRepository;

    private readonly IOrderRepository
        _orderRepository;

    private readonly IEmailLogRepository
        _emailLogRepository;

    private readonly IUnitOfWork
        _unitOfWork;

    private readonly IEmailService
        _emailService;


    public ComplaintService(
        ICustomerRepository customerRepository,
        IComplaintRepository complaintRepository,
        IOrderRepository orderRepository,
        IEmailLogRepository emailLogRepository,
        IUnitOfWork unitOfWork,
        IEmailService emailService)
    {
        _customerRepository =
            customerRepository;

        _complaintRepository =
            complaintRepository;

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

    public async Task<ComplaintDetailResponseDto>
        CreateAsync(
            Guid userId,
            CreateComplaintRequestDto request,
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


        // ========================================================
        // CREATE
        // ========================================================

        var complaint =
            new Complaint(
                customer.Id,
                request.Subject,
                request.Description,
                request.OrderId);

        await _complaintRepository.AddAsync(
            complaint,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);


        // ========================================================
        // RELOAD
        // ========================================================

        var savedComplaint =
            await _complaintRepository
                .GetWithDetailsAsync(
                    complaint.Id,
                    cancellationToken)
            ?? throw new InvalidOperationException(
                "Complaint could not be reloaded.");

        return MapDetail(
            savedComplaint);
    }


    // ============================================================
    // CUSTOMER - LIST
    // ============================================================

    public async Task<List<ComplaintSummaryResponseDto>>
        GetMyComplaintsAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
    {
        var customer =
            await GetCustomerAsync(
                userId,
                cancellationToken);

        var complaints =
            await _complaintRepository
                .GetByCustomerIdAsync(
                    customer.Id,
                    cancellationToken);

        return complaints
            .Select(MapSummary)
            .ToList();
    }


    // ============================================================
    // CUSTOMER - DETAIL
    // ============================================================

    public async Task<ComplaintDetailResponseDto>
        GetMyComplaintAsync(
            Guid userId,
            int complaintId,
            CancellationToken cancellationToken = default)
    {
        var customer =
            await GetCustomerAsync(
                userId,
                cancellationToken);

        var complaint =
            await GetOwnedComplaintAsync(
                customer.Id,
                complaintId,
                cancellationToken);

        return MapDetail(
            complaint);
    }


    // ============================================================
    // ADMIN - LIST
    // ============================================================

    public async Task<List<AdminComplaintSummaryResponseDto>>
        GetAllForAdminAsync(
            CancellationToken cancellationToken = default)
    {
        var complaints =
            await _complaintRepository
                .GetAllAsync(
                    cancellationToken);

        return complaints
            .Select(complaint =>
                new AdminComplaintSummaryResponseDto
                {
                    Id =
                        complaint.Id,

                    CustomerId =
                        complaint.CustomerId,

                    CustomerName =
                        complaint.Customer.FullName,

                    CustomerEmail =
                        complaint.Customer.Email,

                    Subject =
                        complaint.Subject,

                    Status =
                        complaint.Status.ToString(),

                    OrderId =
                        complaint.OrderId,

                    OrderNumber =
                        complaint.Order?.OrderNumber,

                    CreatedAtUtc =
                        complaint.CreatedAtUtc,

                    UpdatedAtUtc =
                        complaint.UpdatedAtUtc
                })
            .ToList();
    }


    // ============================================================
    // ADMIN - DETAIL
    // ============================================================

    public async Task<AdminComplaintDetailResponseDto>
        GetAdminComplaintAsync(
            int complaintId,
            CancellationToken cancellationToken = default)
    {
        var complaint =
            await GetComplaintAsync(
                complaintId,
                cancellationToken);

        return new AdminComplaintDetailResponseDto
        {
            CustomerId =
                complaint.CustomerId,

            CustomerName =
                complaint.Customer.FullName,

            CustomerEmail =
                complaint.Customer.Email,

            Complaint =
                MapDetail(
                    complaint)
        };
    }


    // ============================================================
    // ADMIN - START REVIEW
    // ============================================================

    public async Task<ComplaintDetailResponseDto>
        StartReviewAsync(
            int complaintId,
            CancellationToken cancellationToken = default)
    {
        var complaint =
            await GetComplaintAsync(
                complaintId,
                cancellationToken);

        complaint.StartReview(
            DateTime.UtcNow);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        var subject =
            $"MiniCrm Complaint Update - Complaint #{complaint.Id}";

        var body =
            $"""
            Hello {complaint.Customer.FullName},

            Your complaint is now under review.

            Complaint: #{complaint.Id}
            Subject: {complaint.Subject}
            Status: UnderReview
            """;

        await SendComplaintEmailAsync(
            complaint,
            subject,
            body,
            cancellationToken);

        return MapDetail(
            complaint);
    }


    // ============================================================
    // ADMIN - RESOLVE
    // ============================================================

    public async Task<ComplaintDetailResponseDto>
        ResolveAsync(
            int complaintId,
            ResolveComplaintRequestDto request,
            CancellationToken cancellationToken = default)
    {
        var complaint =
            await GetComplaintAsync(
                complaintId,
                cancellationToken);

        complaint.Resolve(
            request.AdminNote,
            DateTime.UtcNow);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        var subject =
            $"MiniCrm Complaint Resolved - Complaint #{complaint.Id}";

        var body =
            $"""
            Hello {complaint.Customer.FullName},

            Your complaint has been resolved.

            Complaint: #{complaint.Id}
            Subject: {complaint.Subject}
            Status: Resolved

            Resolution:
            {complaint.AdminNote}
            """;

        await SendComplaintEmailAsync(
            complaint,
            subject,
            body,
            cancellationToken);

        return MapDetail(
            complaint);
    }


    // ============================================================
    // ADMIN - REJECT
    // ============================================================

    public async Task<ComplaintDetailResponseDto>
        RejectAsync(
            int complaintId,
            RejectComplaintRequestDto request,
            CancellationToken cancellationToken = default)
    {
        var complaint =
            await GetComplaintAsync(
                complaintId,
                cancellationToken);

        complaint.Reject(
            request.AdminNote,
            DateTime.UtcNow);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        var subject =
            $"MiniCrm Complaint Update - Complaint #{complaint.Id}";

        var body =
            $"""
            Hello {complaint.Customer.FullName},

            Your complaint has been reviewed and rejected.

            Complaint: #{complaint.Id}
            Subject: {complaint.Subject}
            Status: Rejected

            Explanation:
            {complaint.AdminNote}
            """;

        await SendComplaintEmailAsync(
            complaint,
            subject,
            body,
            cancellationToken);

        return MapDetail(
            complaint);
    }


    // ============================================================
    // ADMIN - CLOSE
    // ============================================================

    public async Task<ComplaintDetailResponseDto>
        CloseAsync(
            int complaintId,
            CancellationToken cancellationToken = default)
    {
        var complaint =
            await GetComplaintAsync(
                complaintId,
                cancellationToken);

        complaint.Close(
            DateTime.UtcNow);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        var subject =
            $"MiniCrm Complaint Closed - Complaint #{complaint.Id}";

        var body =
            $"""
            Hello {complaint.Customer.FullName},

            Your complaint record has been closed.

            Complaint: #{complaint.Id}
            Subject: {complaint.Subject}
            Status: Closed
            """;

        await SendComplaintEmailAsync(
            complaint,
            subject,
            body,
            cancellationToken);

        return MapDetail(
            complaint);
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
    // CUSTOMER OWNERSHIP
    // ============================================================

    private async Task<Complaint>
        GetOwnedComplaintAsync(
            int customerId,
            int complaintId,
            CancellationToken cancellationToken)
    {
        var complaint =
            await _complaintRepository
                .GetWithDetailsAsync(
                    complaintId,
                    cancellationToken)
            ?? throw new KeyNotFoundException(
                "Complaint was not found.");

        if (complaint.CustomerId !=
            customerId)
        {
            throw new UnauthorizedAccessException(
                "You do not have access to this complaint.");
        }

        return complaint;
    }


    // ============================================================
    // ADMIN GET
    // ============================================================

    private async Task<Complaint>
        GetComplaintAsync(
            int complaintId,
            CancellationToken cancellationToken)
    {
        return await _complaintRepository
            .GetWithDetailsAsync(
                complaintId,
                cancellationToken)
            ?? throw new KeyNotFoundException(
                "Complaint was not found.");
    }


    // ============================================================
    // EMAIL
    // ============================================================

    private async Task SendComplaintEmailAsync(
        Complaint complaint,
        string subject,
        string body,
        CancellationToken cancellationToken)
    {
        var customer =
            complaint.Customer;

        var emailLog =
            new EmailLog(
                customer.Email,
                subject,
                body,
                EmailType.ComplaintUpdate,
                customer.Id,
                complaintId: complaint.Id);

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

    private static ComplaintSummaryResponseDto
        MapSummary(
            Complaint complaint)
    {
        return new ComplaintSummaryResponseDto
        {
            Id =
                complaint.Id,

            Subject =
                complaint.Subject,

            Status =
                complaint.Status.ToString(),

            OrderId =
                complaint.OrderId,

            CreatedAtUtc =
                complaint.CreatedAtUtc,

            UpdatedAtUtc =
                complaint.UpdatedAtUtc,

            ReviewedAtUtc =
                complaint.ReviewedAtUtc,

            ResolvedAtUtc =
                complaint.ResolvedAtUtc,

            RejectedAtUtc =
                complaint.RejectedAtUtc,

            ClosedAtUtc =
                complaint.ClosedAtUtc
        };
    }


    // ============================================================
    // DETAIL MAP
    // ============================================================

    private static ComplaintDetailResponseDto
        MapDetail(
            Complaint complaint)
    {
        return new ComplaintDetailResponseDto
        {
            Id =
                complaint.Id,

            Subject =
                complaint.Subject,

            Description =
                complaint.Description,

            Status =
                complaint.Status.ToString(),

            OrderId =
                complaint.OrderId,

            OrderNumber =
                complaint.Order?.OrderNumber,

            AdminNote =
                complaint.AdminNote,

            CreatedAtUtc =
                complaint.CreatedAtUtc,

            UpdatedAtUtc =
                complaint.UpdatedAtUtc,

            ReviewedAtUtc =
                complaint.ReviewedAtUtc,

            ResolvedAtUtc =
                complaint.ResolvedAtUtc,

            RejectedAtUtc =
                complaint.RejectedAtUtc,

            ClosedAtUtc =
                complaint.ClosedAtUtc
        };
    }
}