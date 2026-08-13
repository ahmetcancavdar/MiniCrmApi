using MiniCrm.Domain.Common;
using MiniCrm.Domain.Enums;
using MiniCrm.Domain.Exceptions;

namespace MiniCrm.Domain.Entities;

public class StockMovement : BaseEntity
{
    public int ProductId { get; private set; }

    public Product Product { get; private set; } = null!;

    public StockMovementType MovementType { get; private set; }

    public int QuantityChange { get; private set; }

    public int PreviousQuantity { get; private set; }

    public int NewQuantity { get; private set; }

    public string? Note { get; private set; }

    private StockMovement()
    {
    }

    public StockMovement(
        int productId,
        StockMovementType movementType,
        int quantityChange,
        int previousQuantity,
        int newQuantity,
        string? note = null)
    {
        if (productId <= 0)
        {
            throw new DomainException("A valid product is required.");
        }

        if (quantityChange == 0)
        {
            throw new DomainException(
                "Stock movement quantity cannot be zero.");
        }

        if (previousQuantity < 0 || newQuantity < 0)
        {
            throw new DomainException(
                "Stock quantity cannot be negative.");
        }

        ProductId = productId;
        MovementType = movementType;
        QuantityChange = quantityChange;
        PreviousQuantity = previousQuantity;
        NewQuantity = newQuantity;
        Note = note?.Trim();
    }
}