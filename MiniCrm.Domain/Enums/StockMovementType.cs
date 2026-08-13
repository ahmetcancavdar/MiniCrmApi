namespace MiniCrm.Domain.Enums;

public enum StockMovementType
{
    InitialStock = 1,
    AdminIncrease = 2,
    AdminDecrease = 3,
    OrderConfirmed = 4,
    OrderCancelledRestock = 5,
    CustomerReturn = 6
}