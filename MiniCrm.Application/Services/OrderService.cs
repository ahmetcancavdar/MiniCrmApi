using MiniCrm.Application.DTOs.Orders;
using MiniCrm.Application.Interfaces;
using MiniCrm.Application.Interfaces.Repositories;
using MiniCrm.Application.Interfaces.Services;
using MiniCrm.Domain.Entities;
using MiniCrm.Domain.Enums;
using MiniCrm.Domain.ValueObjects;

namespace MiniCrm.Application.Services;

public sealed class OrderService
	: IOrderService
{
	private readonly ICustomerRepository
		_customerRepository;

	private readonly ICartRepository
		_cartRepository;

	private readonly IOrderRepository
		_orderRepository;

	private readonly IStockMovementRepository
		_stockMovementRepository;

	private readonly IEmailLogRepository
		_emailLogRepository;

	private readonly IUnitOfWork
		_unitOfWork;

	private readonly IEmailService
		_emailService;

	private readonly IVerificationCodeService
		_verificationCodeService;

	private readonly IOrderNumberGenerator
		_orderNumberGenerator;

	private readonly IRealtimeNotifier
		_realtimeNotifier;


	public OrderService(
		ICustomerRepository customerRepository,
		ICartRepository cartRepository,
		IOrderRepository orderRepository,
		IStockMovementRepository stockMovementRepository,
		IEmailLogRepository emailLogRepository,
		IUnitOfWork unitOfWork,
		IEmailService emailService,
		IVerificationCodeService verificationCodeService,
		IOrderNumberGenerator orderNumberGenerator,
		IRealtimeNotifier realtimeNotifier)
	{
		_customerRepository =
			customerRepository;

		_cartRepository =
			cartRepository;

		_orderRepository =
			orderRepository;

		_stockMovementRepository =
			stockMovementRepository;

		_emailLogRepository =
			emailLogRepository;

		_unitOfWork =
			unitOfWork;

		_emailService =
			emailService;

		_verificationCodeService =
			verificationCodeService;

		_orderNumberGenerator =
			orderNumberGenerator;

		_realtimeNotifier =
			realtimeNotifier;
	}


	// ============================================================
	// GERÇEK ZAMANLI BİLDİRİMLER (best-effort — hiçbiri asıl işlemi
	// başarısız kılmaz; SMTP gönderiminde uygulanan aynı felsefe)
	// ============================================================

	private async Task NotifyAdminsOrdersUpdatedAsync(
		CancellationToken cancellationToken)
	{
		try
		{
			await _realtimeNotifier.NotifyAdminsAsync(
				"OrdersUpdated",
				true,
				cancellationToken);
		}
		catch
		{
		}
	}

	private async Task NotifyCustomerOrderStatusChangedAsync(
		Guid customerUserId,
		int orderId,
		CancellationToken cancellationToken)
	{
		try
		{
			await _realtimeNotifier.NotifyCustomerAsync(
				customerUserId,
				"OrderStatusChanged",
				orderId,
				cancellationToken);
		}
		catch
		{
		}
	}

	private async Task NotifyProductsUpdatedAsync(
		CancellationToken cancellationToken)
	{
		try
		{
			await _realtimeNotifier.NotifyAllAsync(
				"ProductsUpdated",
				true,
				cancellationToken);
		}
		catch
		{
		}
	}


	// ============================================================
	// CUSTOMER - CHECKOUT
	// ============================================================

	public async Task<CheckoutOrderResponseDto> CheckoutAsync(
		Guid userId,
		CheckoutOrderRequestDto request,
		CancellationToken cancellationToken = default)
	{
		var customer =
			await GetCustomerAsync(
				userId,
				cancellationToken);

		var cart =
			await _cartRepository
				.GetWithItemsAsync(
					customer.Id,
					cancellationToken)
			?? throw new InvalidOperationException(
				"Customer cart was not found.");

		var activeItems =
			cart.Items
				.Where(x => !x.IsDeleted)
				.ToList();

		if (activeItems.Count == 0)
		{
			throw new InvalidOperationException(
				"Cart is empty.");
		}

		foreach (var item in activeItems)
		{
			item.Product.EnsurePurchasable(
				item.Quantity);
		}

		var shippingAddress =
			new OrderAddress(
				request.RecipientName,
				request.Phone,
				request.AddressLine,
				request.City,
				request.District,
				request.PostalCode,
				request.Country);

		var order =
			new Order(
				customer.Id,
				_orderNumberGenerator.Generate(),
				shippingAddress);

		foreach (var item in activeItems)
		{
			order.AddItem(
				item.ProductId,
				item.Product.Name,
				item.Product.SKU,
				item.Product.Price,
				item.Quantity);
		}

		// Sipariş oluşturulduğu an ürünler sepetten kaldırılır; doğrulama
		// bekleniyor olması sepette görünmeye devam etmesini gerektirmez
		// ve müşteri aynı ürünü doğrulamadan önce tekrar sipariş edebilir
		// gibi kafa karıştırıcı bir duruma yol açmaz. Doğrulama başarısız
		// olur/süresi dolarsa müşteri ürünleri tekrar sepete ekleyebilir.
		cart.Clear();

		var utcNow =
			DateTime.UtcNow;

		var code =
			_verificationCodeService.GenerateCode();

		var codeHash =
			_verificationCodeService.HashCode(
				code);

		var expiresAtUtc =
			utcNow.AddMinutes(5);

		order.CreateOrRenewVerification(
			codeHash,
			expiresAtUtc,
			utcNow);

		await _orderRepository.AddAsync(
			order,
			cancellationToken);

		await _unitOfWork.SaveChangesAsync(
			cancellationToken);

		var subject =
			$"MiniCrm Order Verification - {order.OrderNumber}";

		var actualEmailBody =
			$"""
            Your MiniCrm order has been created.

            Order Number: {order.OrderNumber}
            Total Amount: {order.TotalAmount:N2}

            Verification Code: {code}

            This code will expire in 5 minutes.

            If you did not create this order, you can ignore this email.
            """;

		var logBody =
			$"""
            Order verification email.

            Order Number: {order.OrderNumber}
            Verification code: [REDACTED]
            Expiration: {expiresAtUtc:O}
            """;

		var emailSent =
			await SendLoggedEmailAsync(
				customer,
				order,
				EmailType.OrderVerification,
				subject,
				actualEmailBody,
				logBody,
				cancellationToken);

		await NotifyAdminsOrdersUpdatedAsync(
			cancellationToken);

		return new CheckoutOrderResponseDto
		{
			Order =
				Map(order),

			VerificationExpiresAtUtc =
				expiresAtUtc,

			VerificationEmailSent =
				emailSent,

			Message =
				emailSent
					? "Order created. Verification code was sent by email."
					: "Order created, but verification email could not be sent. Request a new verification code."
		};
	}


	// ============================================================
	// CUSTOMER - VERIFY
	// ============================================================

	public async Task<OrderResponseDto> VerifyAsync(
		Guid userId,
		int orderId,
		VerifyOrderRequestDto request,
		CancellationToken cancellationToken = default)
	{
		var customer =
			await GetCustomerAsync(
				userId,
				cancellationToken);

		var order =
			await GetOwnedOrderAsync(
				customer.Id,
				orderId,
				cancellationToken);

		var utcNow =
			DateTime.UtcNow;

		order.EnsureVerificationCanBeAttempted(
			utcNow);

		if (order.Verification is null)
		{
			throw new InvalidOperationException(
				"Order verification was not found.");
		}

		var codeIsValid =
			_verificationCodeService.VerifyCode(
				request.Code,
				order.Verification.CodeHash);

		if (!codeIsValid)
		{
			order.RegisterFailedVerificationAttempt(
				utcNow);

			await _unitOfWork.SaveChangesAsync(
				cancellationToken);

			throw new InvalidOperationException(
				"Verification code is incorrect.");
		}

		foreach (var item in order.Items)
		{
			var product =
				item.Product;

			if (product.StockQuantity <
				item.Quantity)
			{
				throw new InvalidOperationException(
					$"Insufficient stock for '{item.ProductName}'. Available stock: {product.StockQuantity}.");
			}
		}

		order.MarkVerificationSuccessful(
			utcNow);

		foreach (var item in order.Items)
		{
			var product =
				item.Product;

			var previousQuantity =
				product.StockQuantity;

			product.DecreaseStock(
				item.Quantity);

			var movement =
				new StockMovement(
					product.Id,
					StockMovementType.OrderConfirmed,
					-item.Quantity,
					previousQuantity,
					product.StockQuantity,
					$"Order confirmed: {order.OrderNumber}");

			await _stockMovementRepository.AddAsync(
				movement,
				cancellationToken);
		}

		order.Confirm(
			utcNow);

		var cart =
			await _cartRepository
				.GetWithItemsAsync(
					customer.Id,
					cancellationToken);

		cart?.Clear();

		await _unitOfWork.SaveChangesAsync(
			cancellationToken);

		var subject =
			$"MiniCrm Order Confirmed - {order.OrderNumber}";

		var emailBody =
			$"""
            Your order has been successfully verified and confirmed.

            Order Number: {order.OrderNumber}
            Total Amount: {order.TotalAmount:N2}
            Status: Confirmed

            Thank you.
            """;

		await SendLoggedEmailAsync(
			customer,
			order,
			EmailType.OrderConfirmed,
			subject,
			emailBody,
			emailBody,
			cancellationToken);

		await NotifyAdminsOrdersUpdatedAsync(
			cancellationToken);

		await NotifyProductsUpdatedAsync(
			cancellationToken);

		return Map(order);
	}


	// ============================================================
	// CUSTOMER - RESEND VERIFICATION
	// ============================================================

	public async Task<VerificationEmailResponseDto>
		ResendVerificationCodeAsync(
			Guid userId,
			int orderId,
			CancellationToken cancellationToken = default)
	{
		var customer =
			await GetCustomerAsync(
				userId,
				cancellationToken);

		var order =
			await GetOwnedOrderAsync(
				customer.Id,
				orderId,
				cancellationToken);

		var utcNow =
			DateTime.UtcNow;

		var code =
			_verificationCodeService.GenerateCode();

		var hash =
			_verificationCodeService.HashCode(
				code);

		var expiresAtUtc =
			utcNow.AddMinutes(5);

		order.CreateOrRenewVerification(
			hash,
			expiresAtUtc,
			utcNow);

		await _unitOfWork.SaveChangesAsync(
			cancellationToken);

		var subject =
			$"MiniCrm New Verification Code - {order.OrderNumber}";

		var actualEmailBody =
			$"""
            A new verification code was requested.

            Order Number: {order.OrderNumber}

            Verification Code: {code}

            This code will expire in 5 minutes.
            """;

		var logBody =
			$"""
            New order verification email.

            Order Number: {order.OrderNumber}
            Verification code: [REDACTED]
            Expiration: {expiresAtUtc:O}
            """;

		var emailSent =
			await SendLoggedEmailAsync(
				customer,
				order,
				EmailType.OrderVerification,
				subject,
				actualEmailBody,
				logBody,
				cancellationToken);

		return new VerificationEmailResponseDto
		{
			OrderId =
				order.Id,

			OrderNumber =
				order.OrderNumber,

			ExpiresAtUtc =
				expiresAtUtc,

			EmailSent =
				emailSent,

			Message =
				emailSent
					? "A new verification code was sent."
					: "The verification code was renewed, but the email could not be sent."
		};
	}


	// ============================================================
	// CUSTOMER - GET ORDER DETAIL
	// ============================================================

	public async Task<OrderResponseDto> GetByIdAsync(
		Guid userId,
		int orderId,
		CancellationToken cancellationToken = default)
	{
		var customer =
			await GetCustomerAsync(
				userId,
				cancellationToken);

		var order =
			await GetOwnedOrderAsync(
				customer.Id,
				orderId,
				cancellationToken);

		return Map(order);
	}


	// ============================================================
	// CUSTOMER - CANCEL
	// ============================================================

	public async Task<OrderResponseDto> CancelAsync(
		Guid userId,
		int orderId,
		CancelOrderRequestDto request,
		CancellationToken cancellationToken = default)
	{
		var customer =
			await GetCustomerAsync(
				userId,
				cancellationToken);

		var order =
			await GetOwnedOrderAsync(
				customer.Id,
				orderId,
				cancellationToken);

		var previousStatus =
			order.Status;

		var requiresRestock =
			previousStatus is
				OrderStatus.Confirmed
				or OrderStatus.Preparing;

		if (requiresRestock)
		{
			foreach (var item in order.Items)
			{
				var product =
					item.Product;

				var previousQuantity =
					product.StockQuantity;

				product.IncreaseStock(
					item.Quantity);

				var movement =
					new StockMovement(
						product.Id,
						StockMovementType.OrderCancelledRestock,
						item.Quantity,
						previousQuantity,
						product.StockQuantity,
						$"Order cancelled by customer: {order.OrderNumber}. Reason: {request.Reason.Trim()}");

				await _stockMovementRepository.AddAsync(
					movement,
					cancellationToken);
			}
		}

		order.Cancel(
			DateTime.UtcNow,
			request.Reason);

		await _unitOfWork.SaveChangesAsync(
			cancellationToken);

		var emailMessage =
			$"""
            Your order has been cancelled.

            Order Number: {order.OrderNumber}
            Reason: {order.CancellationReason}
            """;

		await SendStatusEmailAsync(
			order,
			"Cancelled",
			emailMessage,
			cancellationToken);

		await NotifyAdminsOrdersUpdatedAsync(
			cancellationToken);

		if (requiresRestock)
		{
			await NotifyProductsUpdatedAsync(
				cancellationToken);
		}

		return Map(order);
	}


	// ============================================================
	// CUSTOMER - GET MY ORDERS
	// ============================================================

	public async Task<List<OrderSummaryResponseDto>>
		GetMyOrdersAsync(
			Guid userId,
			CancellationToken cancellationToken = default)
	{
		var customer =
			await GetCustomerAsync(
				userId,
				cancellationToken);

		var orders =
			await _orderRepository
				.GetByCustomerIdAsync(
					customer.Id,
					cancellationToken);

		return orders
			.Select(MapSummary)
			.ToList();
	}


	// ============================================================
	// ADMIN - GET ALL ORDERS
	// ============================================================

	public async Task<List<AdminOrderSummaryResponseDto>>
		GetAllForAdminAsync(
			CancellationToken cancellationToken = default)
	{
		var orders =
			await _orderRepository.GetAllAsync(
				cancellationToken);

		return orders
			.Select(order =>
				new AdminOrderSummaryResponseDto
				{
					Id =
						order.Id,

					OrderNumber =
						order.OrderNumber,

					CustomerId =
						order.CustomerId,

					CustomerName =
						order.Customer.FullName,

					CustomerEmail =
						order.Customer.Email,

					Status =
						order.Status.ToString(),

					TotalAmount =
						order.TotalAmount,

					CreatedAtUtc =
						order.CreatedAtUtc,

					ConfirmedAtUtc =
						order.ConfirmedAtUtc,

					ShippedAtUtc =
						order.ShippedAtUtc,

					DeliveredAtUtc =
						order.DeliveredAtUtc,

					CancelledAtUtc =
						order.CancelledAtUtc,

					RecipientName =
						order.ShippingAddress.RecipientName,

					Phone =
						order.ShippingAddress.Phone,

					AddressLine =
						order.ShippingAddress.AddressLine,

					City =
						order.ShippingAddress.City,

					District =
						order.ShippingAddress.District,

					PostalCode =
						order.ShippingAddress.PostalCode,

					Country =
						order.ShippingAddress.Country
				})
			.ToList();
	}


	// ============================================================
	// ADMIN - GET DETAIL
	// ============================================================

	public async Task<AdminOrderDetailResponseDto>
		GetAdminByIdAsync(
			int orderId,
			CancellationToken cancellationToken = default)
	{
		var order =
			await GetOrderWithDetailsAsync(
				orderId,
				cancellationToken);

		return new AdminOrderDetailResponseDto
		{
			CustomerId =
				order.CustomerId,

			CustomerName =
				order.Customer.FullName,

			CustomerEmail =
				order.Customer.Email,

			Order =
				Map(order)
		};
	}


	// ============================================================
	// ADMIN - START PREPARING
	// ============================================================

	public async Task<OrderResponseDto> StartPreparingAsync(
		int orderId,
		CancellationToken cancellationToken = default)
	{
		var order =
			await GetOrderWithDetailsAsync(
				orderId,
				cancellationToken);

		order.StartPreparing();

		await _unitOfWork.SaveChangesAsync(
			cancellationToken);

		await SendStatusEmailAsync(
			order,
			"Preparing",
			"Your order is now being prepared.",
			cancellationToken);

		await NotifyAdminsOrdersUpdatedAsync(
			cancellationToken);

		await NotifyCustomerOrderStatusChangedAsync(
			order.Customer.UserId,
			order.Id,
			cancellationToken);

		return Map(order);
	}


	// ============================================================
	// ADMIN - SHIP
	// ============================================================

	public async Task<OrderResponseDto> MarkAsShippedAsync(
		int orderId,
		CancellationToken cancellationToken = default)
	{
		var order =
			await GetOrderWithDetailsAsync(
				orderId,
				cancellationToken);

		order.MarkAsShipped(
			DateTime.UtcNow);

		await _unitOfWork.SaveChangesAsync(
			cancellationToken);

		await SendStatusEmailAsync(
			order,
			"Shipped",
			"Your order has been shipped.",
			cancellationToken);

		await NotifyAdminsOrdersUpdatedAsync(
			cancellationToken);

		await NotifyCustomerOrderStatusChangedAsync(
			order.Customer.UserId,
			order.Id,
			cancellationToken);

		return Map(order);
	}


	// ============================================================
	// ADMIN - DELIVER
	// ============================================================

	public async Task<OrderResponseDto> MarkAsDeliveredAsync(
		int orderId,
		CancellationToken cancellationToken = default)
	{
		var order =
			await GetOrderWithDetailsAsync(
				orderId,
				cancellationToken);

		order.MarkAsDelivered(
			DateTime.UtcNow);

		await _unitOfWork.SaveChangesAsync(
			cancellationToken);

		await SendStatusEmailAsync(
			order,
			"Delivered",
			"Your order has been delivered.",
			cancellationToken);

		await NotifyAdminsOrdersUpdatedAsync(
			cancellationToken);

		await NotifyCustomerOrderStatusChangedAsync(
			order.Customer.UserId,
			order.Id,
			cancellationToken);

		return Map(order);
	}


	// ============================================================
	// ADMIN - CANCEL
	// ============================================================

	public async Task<OrderResponseDto> CancelByAdminAsync(
		int orderId,
		CancelOrderRequestDto request,
		CancellationToken cancellationToken = default)
	{
		var order =
			await GetOrderWithDetailsAsync(
				orderId,
				cancellationToken);

		var previousStatus =
			order.Status;

		var requiresRestock =
			previousStatus is
				OrderStatus.Confirmed
				or OrderStatus.Preparing;

		if (requiresRestock)
		{
			foreach (var item in order.Items)
			{
				var product =
					item.Product;

				var previousQuantity =
					product.StockQuantity;

				product.IncreaseStock(
					item.Quantity);

				var movement =
					new StockMovement(
						product.Id,
						StockMovementType.OrderCancelledRestock,
						item.Quantity,
						previousQuantity,
						product.StockQuantity,
						$"Order cancelled: {order.OrderNumber}. Reason: {request.Reason.Trim()}");

				await _stockMovementRepository.AddAsync(
					movement,
					cancellationToken);
			}
		}

		order.Cancel(
			DateTime.UtcNow,
			request.Reason);

		await _unitOfWork.SaveChangesAsync(
			cancellationToken);

		var emailMessage =
			$"""
            Your order has been cancelled.

            Order Number: {order.OrderNumber}
            Reason: {order.CancellationReason}
            """;

		await SendStatusEmailAsync(
			order,
			"Cancelled",
			emailMessage,
			cancellationToken);

		await NotifyAdminsOrdersUpdatedAsync(
			cancellationToken);

		await NotifyCustomerOrderStatusChangedAsync(
			order.Customer.UserId,
			order.Id,
			cancellationToken);

		if (requiresRestock)
		{
			await NotifyProductsUpdatedAsync(
				cancellationToken);
		}

		return Map(order);
	}


	// ============================================================
	// CUSTOMER HELPER
	// ============================================================

	private async Task<Customer> GetCustomerAsync(
		Guid userId,
		CancellationToken cancellationToken)
	{
		if (userId == Guid.Empty)
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

	private async Task<Order> GetOwnedOrderAsync(
		int customerId,
		int orderId,
		CancellationToken cancellationToken)
	{
		var order =
			await _orderRepository
				.GetWithDetailsAsync(
					orderId,
					cancellationToken)
			?? throw new KeyNotFoundException(
				"Order was not found.");

		if (order.CustomerId !=
			customerId)
		{
			throw new UnauthorizedAccessException(
				"You do not have access to this order.");
		}

		return order;
	}


	// ============================================================
	// ADMIN ORDER DETAIL
	// ============================================================

	private async Task<Order> GetOrderWithDetailsAsync(
		int orderId,
		CancellationToken cancellationToken)
	{
		return await _orderRepository
			.GetWithDetailsAsync(
				orderId,
				cancellationToken)
			?? throw new KeyNotFoundException(
				"Order was not found.");
	}


	// ============================================================
	// STATUS EMAIL
	// ============================================================

	private async Task SendStatusEmailAsync(
		Order order,
		string statusName,
		string message,
		CancellationToken cancellationToken)
	{
		var customer =
			order.Customer;

		var subject =
			$"MiniCrm Order Status - {order.OrderNumber} - {statusName}";

		var body =
			$"""
            Order Number: {order.OrderNumber}

            Status: {statusName}

            {message}
            """;

		await SendLoggedEmailAsync(
			customer,
			order,
			EmailType.OrderStatusChanged,
			subject,
			body,
			body,
			cancellationToken);
	}


	// ============================================================
	// EMAIL + EMAIL LOG
	// ============================================================

	private async Task<bool> SendLoggedEmailAsync(
		Customer customer,
		Order order,
		EmailType emailType,
		string subject,
		string actualEmailBody,
		string logBody,
		CancellationToken cancellationToken)
	{
		var emailLog =
			new EmailLog(
				customer.Email,
				subject,
				logBody,
				emailType,
				customer.Id,
				order.Id);

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
				actualEmailBody,
				cancellationToken);

			emailLog.MarkAsSent(
				DateTime.UtcNow);

			await _unitOfWork.SaveChangesAsync(
				cancellationToken);

			return true;
		}
		catch (Exception exception)
		{
			emailLog.MarkAsFailed(
				exception.Message,
				DateTime.UtcNow);

			await _unitOfWork.SaveChangesAsync(
				cancellationToken);

			return false;
		}
	}


	// ============================================================
	// ORDER DETAIL MAP
	// ============================================================

	private static OrderResponseDto Map(
		Order order)
	{
		return new OrderResponseDto
		{
			Id =
				order.Id,

			OrderNumber =
				order.OrderNumber,

			Status =
				order.Status.ToString(),

			TotalAmount =
				order.TotalAmount,

			CreatedAtUtc =
				order.CreatedAtUtc,

			RecipientName =
				order.ShippingAddress.RecipientName,

			Phone =
				order.ShippingAddress.Phone,

			AddressLine =
				order.ShippingAddress.AddressLine,

			City =
				order.ShippingAddress.City,

			District =
				order.ShippingAddress.District,

			PostalCode =
				order.ShippingAddress.PostalCode,

			Country =
				order.ShippingAddress.Country,

			VerificationExpiresAtUtc =
				order.Verification?.ExpiresAtUtc,

			ConfirmedAtUtc =
				order.ConfirmedAtUtc,

			ShippedAtUtc =
				order.ShippedAtUtc,

			DeliveredAtUtc =
				order.DeliveredAtUtc,

			CancelledAtUtc =
				order.CancelledAtUtc,

			CancellationReason =
				order.CancellationReason,

			Items =
				order.Items
					.Select(item =>
						new OrderItemResponseDto
						{
							ProductId =
								item.ProductId,

							ProductName =
								item.ProductName,

							SKU =
								item.SKU,

							Quantity =
								item.Quantity,

							UnitPrice =
								item.UnitPrice,

							LineTotal =
								item.LineTotal
						})
					.ToList()
		};
	}


	// ============================================================
	// ORDER SUMMARY MAP
	// ============================================================

	private static OrderSummaryResponseDto MapSummary(
		Order order)
	{
		return new OrderSummaryResponseDto
		{
			Id =
				order.Id,

			OrderNumber =
				order.OrderNumber,

			Status =
				order.Status.ToString(),

			TotalAmount =
				order.TotalAmount,

			CreatedAtUtc =
				order.CreatedAtUtc,

			ConfirmedAtUtc =
				order.ConfirmedAtUtc,

			ShippedAtUtc =
				order.ShippedAtUtc,

			DeliveredAtUtc =
				order.DeliveredAtUtc,

			CancelledAtUtc =
				order.CancelledAtUtc
		};
	}
}