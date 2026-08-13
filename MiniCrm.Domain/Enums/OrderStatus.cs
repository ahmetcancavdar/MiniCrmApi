namespace MiniCrm.Domain.Enums;

public enum OrderStatus
{
    PendingVerification = 1,
    Confirmed = 2,
    Preparing = 3,
    Shipped = 4,
    Delivered = 5,
    Cancelled = 6
}