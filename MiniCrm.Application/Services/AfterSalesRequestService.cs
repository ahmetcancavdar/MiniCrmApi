using MiniCrm.Application.DTOs.AfterSales;
using MiniCrm.Application.Interfaces;
using MiniCrm.Application.Interfaces.Repositories;
using MiniCrm.Application.Interfaces.Services;
using MiniCrm.Domain.Entities;
using MiniCrm.Domain.Enums;

namespace MiniCrm.Application.Services;

public sealed class AfterSalesRequestService
	: IAfterSalesRequestService
{
	private readonly ICustomerRepository
		_customerRepository;

	private readonly IOrderRepository
		_orderRepository;

	private readonly IAfterSalesRequestRepository
		_afterSalesRepository;

	private readonly IEmailLogRepository
		_emailLogRepository;

	private readonly IUnitOfWork
		_unitOfWork;

	private readonly IEmailService
		_emailService;


	public AfterSalesRequestService(
		ICustomerRepository customerRepository,
		IOrderRepository orderRepository,
		IAfterSalesRequestRepository afterSalesRepository,
		IEmailLogRepository emailLogRepository,
		IUnitOfWork unitOfWork,
		IEmailService emailService)
	{
		_customerRepository =
			customerRepository;

		_orderRepository =
			orderRepository;

		_afterSalesRepository =
			afterSalesRepository;

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

	public async Task<AfterSalesRequestDetailResponseDto>
		CreateAsync(
			Guid userId,
			CreateAfterSalesRequestDto request,
			CancellationToken cancellationToken = default)
	{
		var customer =
			await GetCustomerAsync(
				userId,
				cancellationToken);


		// ========================================================
		// REQUEST TYPE
		// ========================================================

		if (!Enum.IsDefined(
				typeof(AfterSalesRequestType),
				request.RequestType))
		{
			throw new InvalidOperationException(
				"Invalid after-sales request type.");
		}


		// ========================================================
		// ORDER
		// ========================================================

		var order =
			await _orderRepository
				.GetWithDetailsAsync(
					request.OrderId,
					cancellationToken)
			?? throw new KeyNotFoundException(
				"Order was not found.");

		if (order.CustomerId !=
			customer.Id)
		{
			throw new UnauthorizedAccessException(
				"You do not have access to this order.");
		}


		// ========================================================
		// ONLY DELIVERED ORDERS
		// ========================================================

		if (order.Status !=
			OrderStatus.Delivered)
		{
			throw new InvalidOperationException(
				"After-sales requests can only be created for delivered orders.");
		}


		// ========================================================
		// FIND ORDER ITEM BY PRODUCT
		// ========================================================

		var orderItem =
			order.Items
				.FirstOrDefault(x =>
					x.ProductId ==
					request.ProductId)
			?? throw new KeyNotFoundException(
				"The selected product was not found in this order.");


		// ========================================================
		// QUANTITY
		// ========================================================

		if (request.Quantity <= 0)
		{
			throw new InvalidOperationException(
				"Quantity must be greater than zero.");
		}

		if (request.Quantity >
			orderItem.Quantity)
		{
			throw new InvalidOperationException(
				"Requested quantity cannot exceed the purchased quantity.");
		}


		// ========================================================
		// DUPLICATE ACTIVE REQUEST
		// ========================================================

		var hasActiveRequest =
			await _afterSalesRepository
				.HasActiveRequestForOrderItemAsync(
					customer.Id,
					orderItem.Id,
					cancellationToken);

		if (hasActiveRequest)
		{
			throw new InvalidOperationException(
				"There is already an active after-sales request for this order item.");
		}


		// ========================================================
		// CREATE
		// ========================================================

		var afterSalesRequest =
			new AfterSalesRequest(
				customer.Id,
				orderItem.Id,
				request.RequestType,
				request.Quantity,
				request.Reason);

		await _afterSalesRepository.AddAsync(
			afterSalesRequest,
			cancellationToken);

		await _unitOfWork.SaveChangesAsync(
			cancellationToken);


		// ========================================================
		// RELOAD
		// ========================================================

		var savedRequest =
			await _afterSalesRepository
				.GetWithDetailsAsync(
					afterSalesRequest.Id,
					cancellationToken)
			?? throw new InvalidOperationException(
				"After-sales request could not be reloaded.");


		// ========================================================
		// EMAIL
		// ========================================================

		await SendAfterSalesEmailAsync(
			savedRequest,
			$"MiniCrm After-Sales Request #{savedRequest.Id}",
			$"""
            Hello {savedRequest.Customer.FullName},

            Your after-sales request has been received.

            Request: #{savedRequest.Id}
            Order: {savedRequest.OrderItem.Order.OrderNumber}
            Product: {savedRequest.OrderItem.ProductName}
            Type: {savedRequest.RequestType}
            Quantity: {savedRequest.Quantity}
            Status: Requested
            """,
			cancellationToken);

		return MapDetail(
			savedRequest);
	}


	// ============================================================
	// CUSTOMER - LIST
	// ============================================================

	public async Task<List<AfterSalesRequestSummaryResponseDto>>
		GetMyRequestsAsync(
			Guid userId,
			CancellationToken cancellationToken = default)
	{
		var customer =
			await GetCustomerAsync(
				userId,
				cancellationToken);

		var requests =
			await _afterSalesRepository
				.GetByCustomerIdAsync(
					customer.Id,
					cancellationToken);

		return requests
			.Select(MapSummary)
			.ToList();
	}


	// ============================================================
	// CUSTOMER - DETAIL
	// ============================================================

	public async Task<AfterSalesRequestDetailResponseDto>
		GetMyRequestAsync(
			Guid userId,
			int requestId,
			CancellationToken cancellationToken = default)
	{
		var customer =
			await GetCustomerAsync(
				userId,
				cancellationToken);

		var request =
			await GetOwnedRequestAsync(
				customer.Id,
				requestId,
				cancellationToken);

		return MapDetail(
			request);
	}


	// ============================================================
	// CUSTOMER - CANCEL
	// ============================================================

	public async Task<AfterSalesRequestDetailResponseDto>
		CancelAsync(
			Guid userId,
			int requestId,
			CancellationToken cancellationToken = default)
	{
		var customer =
			await GetCustomerAsync(
				userId,
				cancellationToken);

		var request =
			await GetOwnedRequestAsync(
				customer.Id,
				requestId,
				cancellationToken);

		request.Cancel(
			DateTime.UtcNow);

		await _unitOfWork.SaveChangesAsync(
			cancellationToken);

		await SendAfterSalesEmailAsync(
			request,
			$"MiniCrm After-Sales Request Cancelled #{request.Id}",
			$"""
            Hello {request.Customer.FullName},

            Your after-sales request has been cancelled.

            Request: #{request.Id}
            Order: {request.OrderItem.Order.OrderNumber}
            Product: {request.OrderItem.ProductName}
            Status: Cancelled
            """,
			cancellationToken);

		return MapDetail(
			request);
	}


	// ============================================================
	// ADMIN - LIST
	// ============================================================

	public async Task<List<AdminAfterSalesRequestSummaryResponseDto>>
		GetAllForAdminAsync(
			CancellationToken cancellationToken = default)
	{
		var requests =
			await _afterSalesRepository
				.GetAllAsync(
					cancellationToken);

		return requests
			.Select(request =>
				new AdminAfterSalesRequestSummaryResponseDto
				{
					Id =
						request.Id,

					CustomerId =
						request.CustomerId,

					CustomerName =
						request.Customer.FullName,

					CustomerEmail =
						request.Customer.Email,

					RequestType =
						request.RequestType.ToString(),

					Status =
						request.Status.ToString(),

					OrderId =
						request.OrderItem.OrderId,

					OrderNumber =
						request.OrderItem.Order.OrderNumber,

					ProductId =
						request.OrderItem.ProductId,

					ProductName =
						request.OrderItem.ProductName,

					RequestedQuantity =
						request.Quantity,

					CreatedAtUtc =
						request.CreatedAtUtc
				})
			.ToList();
	}


	// ============================================================
	// ADMIN - DETAIL
	// ============================================================

	public async Task<AdminAfterSalesRequestDetailResponseDto>
		GetAdminRequestAsync(
			int requestId,
			CancellationToken cancellationToken = default)
	{
		var request =
			await GetRequestAsync(
				requestId,
				cancellationToken);

		return new AdminAfterSalesRequestDetailResponseDto
		{
			CustomerId =
				request.CustomerId,

			CustomerName =
				request.Customer.FullName,

			CustomerEmail =
				request.Customer.Email,

			Request =
				MapDetail(
					request)
		};
	}


	// ============================================================
	// ADMIN - REVIEW
	// ============================================================

	public async Task<AfterSalesRequestDetailResponseDto>
		StartReviewAsync(
			int requestId,
			CancellationToken cancellationToken = default)
	{
		var request =
			await GetRequestAsync(
				requestId,
				cancellationToken);

		request.StartReview(
			DateTime.UtcNow);

		await _unitOfWork.SaveChangesAsync(
			cancellationToken);

		await SendAfterSalesEmailAsync(
			request,
			$"MiniCrm After-Sales Update #{request.Id}",
			$"""
            Hello {request.Customer.FullName},

            Your after-sales request is now under review.

            Request: #{request.Id}
            Order: {request.OrderItem.Order.OrderNumber}
            Product: {request.OrderItem.ProductName}
            Type: {request.RequestType}
            Status: UnderReview
            """,
			cancellationToken);

		return MapDetail(
			request);
	}


	// ============================================================
	// ADMIN - APPROVE
	// ============================================================

	public async Task<AfterSalesRequestDetailResponseDto>
		ApproveAsync(
			int requestId,
			AfterSalesDecisionRequestDto decision,
			CancellationToken cancellationToken = default)
	{
		var request =
			await GetRequestAsync(
				requestId,
				cancellationToken);

		request.Approve(
			decision.AdminNote);

		await _unitOfWork.SaveChangesAsync(
			cancellationToken);

		await SendAfterSalesEmailAsync(
			request,
			$"MiniCrm After-Sales Request Approved #{request.Id}",
			$"""
            Hello {request.Customer.FullName},

            Your after-sales request has been approved.

            Request: #{request.Id}
            Order: {request.OrderItem.Order.OrderNumber}
            Product: {request.OrderItem.ProductName}
            Type: {request.RequestType}
            Status: Approved

            Admin Note:
            {request.AdminNote}
            """,
			cancellationToken);

		return MapDetail(
			request);
	}


	// ============================================================
	// ADMIN - REJECT
	// ============================================================

	public async Task<AfterSalesRequestDetailResponseDto>
		RejectAsync(
			int requestId,
			AfterSalesDecisionRequestDto decision,
			CancellationToken cancellationToken = default)
	{
		var request =
			await GetRequestAsync(
				requestId,
				cancellationToken);

		request.Reject(
			decision.AdminNote);

		await _unitOfWork.SaveChangesAsync(
			cancellationToken);

		await SendAfterSalesEmailAsync(
			request,
			$"MiniCrm After-Sales Request Rejected #{request.Id}",
			$"""
            Hello {request.Customer.FullName},

            Your after-sales request has been reviewed and rejected.

            Request: #{request.Id}
            Order: {request.OrderItem.Order.OrderNumber}
            Product: {request.OrderItem.ProductName}
            Type: {request.RequestType}
            Status: Rejected

            Explanation:
            {request.AdminNote}
            """,
			cancellationToken);

		return MapDetail(
			request);
	}


	// ============================================================
	// ADMIN - COMPLETE
	// ============================================================

	public async Task<AfterSalesRequestDetailResponseDto>
		CompleteAsync(
			int requestId,
			CancellationToken cancellationToken = default)
	{
		var request =
			await GetRequestAsync(
				requestId,
				cancellationToken);

		request.Complete(
			DateTime.UtcNow);

		await _unitOfWork.SaveChangesAsync(
			cancellationToken);

		await SendAfterSalesEmailAsync(
			request,
			$"MiniCrm After-Sales Request Completed #{request.Id}",
			$"""
            Hello {request.Customer.FullName},

            Your after-sales request has been completed.

            Request: #{request.Id}
            Order: {request.OrderItem.Order.OrderNumber}
            Product: {request.OrderItem.ProductName}
            Type: {request.RequestType}
            Status: Completed
            """,
			cancellationToken);

		return MapDetail(
			request);
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

	private async Task<AfterSalesRequest>
		GetOwnedRequestAsync(
			int customerId,
			int requestId,
			CancellationToken cancellationToken)
	{
		var request =
			await _afterSalesRepository
				.GetWithDetailsAsync(
					requestId,
					cancellationToken)
			?? throw new KeyNotFoundException(
				"After-sales request was not found.");

		if (request.CustomerId !=
			customerId)
		{
			throw new UnauthorizedAccessException(
				"You do not have access to this after-sales request.");
		}

		return request;
	}


	// ============================================================
	// ADMIN GET
	// ============================================================

	private async Task<AfterSalesRequest>
		GetRequestAsync(
			int requestId,
			CancellationToken cancellationToken)
	{
		return await _afterSalesRepository
			.GetWithDetailsAsync(
				requestId,
				cancellationToken)
			?? throw new KeyNotFoundException(
				"After-sales request was not found.");
	}


	// ============================================================
	// EMAIL
	// ============================================================

	private async Task SendAfterSalesEmailAsync(
		AfterSalesRequest request,
		string subject,
		string body,
		CancellationToken cancellationToken)
	{
		var emailLog =
			new EmailLog(
				request.Customer.Email,
				subject,
				body,
				EmailType.AfterSalesUpdate,
				request.CustomerId,
				afterSalesRequestId:
					request.Id);

		await _emailLogRepository.AddAsync(
			emailLog,
			cancellationToken);

		await _unitOfWork.SaveChangesAsync(
			cancellationToken);

		try
		{
			await _emailService.SendAsync(
				request.Customer.Email,
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

	private static AfterSalesRequestSummaryResponseDto
		MapSummary(
			AfterSalesRequest request)
	{
		return new AfterSalesRequestSummaryResponseDto
		{
			Id =
				request.Id,

			RequestType =
				request.RequestType.ToString(),

			Status =
				request.Status.ToString(),

			OrderId =
				request.OrderItem.OrderId,

			OrderNumber =
				request.OrderItem.Order.OrderNumber,

			ProductId =
				request.OrderItem.ProductId,

			ProductName =
				request.OrderItem.ProductName,

			SKU =
				request.OrderItem.SKU,

			RequestedQuantity =
				request.Quantity,

			CreatedAtUtc =
				request.CreatedAtUtc,

			ReviewedAtUtc =
				request.ReviewedAtUtc,

			CompletedAtUtc =
				request.CompletedAtUtc,

			CancelledAtUtc =
				request.CancelledAtUtc
		};
	}


	// ============================================================
	// DETAIL MAP
	// ============================================================

	private static AfterSalesRequestDetailResponseDto
		MapDetail(
			AfterSalesRequest request)
	{
		return new AfterSalesRequestDetailResponseDto
		{
			Id =
				request.Id,

			OrderItemId =
				request.OrderItemId,

			RequestType =
				request.RequestType.ToString(),

			Status =
				request.Status.ToString(),

			OrderId =
				request.OrderItem.OrderId,

			OrderNumber =
				request.OrderItem.Order.OrderNumber,

			ProductId =
				request.OrderItem.ProductId,

			ProductName =
				request.OrderItem.ProductName,

			SKU =
				request.OrderItem.SKU,

			PurchasedQuantity =
				request.OrderItem.Quantity,

			RequestedQuantity =
				request.Quantity,

			Reason =
				request.Reason,

			AdminNote =
				request.AdminNote,

			CreatedAtUtc =
				request.CreatedAtUtc,

			UpdatedAtUtc =
				request.UpdatedAtUtc,

			ReviewedAtUtc =
				request.ReviewedAtUtc,

			CompletedAtUtc =
				request.CompletedAtUtc,

			CancelledAtUtc =
				request.CancelledAtUtc
		};
	}
}