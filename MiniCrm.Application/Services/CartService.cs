using MiniCrm.Application.DTOs.Cart;
using MiniCrm.Application.Interfaces;
using MiniCrm.Application.Interfaces.Repositories;
using MiniCrm.Application.Interfaces.Services;
using MiniCrm.Domain.Entities;

namespace MiniCrm.Application.Services;

public sealed class CartService : ICartService
{
    private readonly ICustomerRepository
        _customerRepository;

    private readonly ICartRepository
        _cartRepository;

    private readonly IProductRepository
        _productRepository;

    private readonly IUnitOfWork
        _unitOfWork;

    public CartService(
        ICustomerRepository customerRepository,
        ICartRepository cartRepository,
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _customerRepository =
            customerRepository;

        _cartRepository =
            cartRepository;

        _productRepository =
            productRepository;

        _unitOfWork =
            unitOfWork;
    }

    // ============================================================
    // GET CART
    // ============================================================

    public async Task<CartResponseDto> GetAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var customer =
            await GetCustomerAsync(
                userId,
                cancellationToken);

        var cart =
            await GetCartAsync(
                customer.Id,
                cancellationToken);

        return Map(cart);
    }


    // ============================================================
    // ADD PRODUCT
    // ============================================================

    public async Task<CartResponseDto> AddItemAsync(
        Guid userId,
        AddCartItemRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var customer =
            await GetCustomerAsync(
                userId,
                cancellationToken);

        var cart =
            await GetCartAsync(
                customer.Id,
                cancellationToken);

        var product =
            await _productRepository.GetByIdAsync(
                request.ProductId,
                cancellationToken)
            ?? throw new KeyNotFoundException(
                "Product was not found.");

        var existingItem =
            cart.Items.FirstOrDefault(x =>
                x.ProductId == product.Id &&
                !x.IsDeleted);

        var currentQuantity =
            existingItem?.Quantity ?? 0;

        var requestedTotalQuantity =
            currentQuantity +
            request.Quantity;

        product.EnsurePurchasable(
            requestedTotalQuantity);

        cart.AddProduct(
            product.Id,
            request.Quantity);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return await ReloadAndMapAsync(
            customer.Id,
            cancellationToken);
    }


    // ============================================================
    // CHANGE QUANTITY
    // ============================================================

    public async Task<CartResponseDto> UpdateQuantityAsync(
        Guid userId,
        int productId,
        UpdateCartItemQuantityRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var customer =
            await GetCustomerAsync(
                userId,
                cancellationToken);

        var cart =
            await GetCartAsync(
                customer.Id,
                cancellationToken);

        var product =
            await _productRepository.GetByIdAsync(
                productId,
                cancellationToken)
            ?? throw new KeyNotFoundException(
                "Product was not found.");

        product.EnsurePurchasable(
            request.Quantity);

        cart.ChangeProductQuantity(
            product.Id,
            request.Quantity);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return await ReloadAndMapAsync(
            customer.Id,
            cancellationToken);
    }


    // ============================================================
    // REMOVE ITEM
    // ============================================================

    public async Task<CartResponseDto> RemoveItemAsync(
        Guid userId,
        int productId,
        CancellationToken cancellationToken = default)
    {
        var customer =
            await GetCustomerAsync(
                userId,
                cancellationToken);

        var cart =
            await GetCartAsync(
                customer.Id,
                cancellationToken);

        cart.RemoveProduct(
            productId);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return await ReloadAndMapAsync(
            customer.Id,
            cancellationToken);
    }


    // ============================================================
    // CLEAR CART
    // ============================================================

    public async Task<CartResponseDto> ClearAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var customer =
            await GetCustomerAsync(
                userId,
                cancellationToken);

        var cart =
            await GetCartAsync(
                customer.Id,
                cancellationToken);

        cart.Clear();

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return await ReloadAndMapAsync(
            customer.Id,
            cancellationToken);
    }


    // ============================================================
    // CUSTOMER
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
    // CART
    // ============================================================

    private async Task<Domain.Entities.Cart> GetCartAsync(
        int customerId,
        CancellationToken cancellationToken)
    {
        return await _cartRepository
            .GetWithItemsAsync(
                customerId,
                cancellationToken)
            ?? throw new InvalidOperationException(
                "Customer cart was not found.");
    }


    // ============================================================
    // RELOAD
    // ============================================================

    private async Task<CartResponseDto> ReloadAndMapAsync(
        int customerId,
        CancellationToken cancellationToken)
    {
        var cart =
            await _cartRepository.GetWithItemsAsync(
                customerId,
                cancellationToken)
            ?? throw new InvalidOperationException(
                "Customer cart was not found.");

        return Map(cart);
    }


    // ============================================================
    // MAP
    // ============================================================

    private static CartResponseDto Map(
        Domain.Entities.Cart cart)
    {
        var activeItems =
            cart.Items
                .Where(x => !x.IsDeleted)
                .ToList();

        var itemDtos =
            activeItems
                .Select(item =>
                {
                    var product =
                        item.Product;

                    var isAvailable =
                        product.IsPurchasable &&
                        product.StockQuantity >=
                            item.Quantity;

                    return new CartItemResponseDto
                    {
                        ProductId =
                            product.Id,

                        ProductName =
                            product.Name,

                        SKU =
                            product.SKU,

                        UnitPrice =
                            product.Price,

                        Quantity =
                            item.Quantity,

                        LineTotal =
                            product.Price *
                            item.Quantity,

                        AvailableStock =
                            product.StockQuantity,

                        ImageUrl =
                            product.ImageUrl,

                        IsAvailable =
                            isAvailable
                    };
                })
                .ToList();

        return new CartResponseDto
        {
            CartId =
                cart.Id,

            CustomerId =
                cart.CustomerId,

            TotalItemCount =
                itemDtos.Sum(x => x.Quantity),

            TotalAmount =
                itemDtos.Sum(x => x.LineTotal),

            Items =
                itemDtos
        };
    }
}