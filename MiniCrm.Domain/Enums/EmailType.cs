namespace MiniCrm.Domain.Enums;

public enum EmailType
{
    OrderVerification = 1,
    OrderConfirmed = 2,
    OrderStatusChanged = 3,
    TicketUpdate = 4,
    ComplaintUpdate = 5,
    AfterSalesUpdate = 6,
    General = 7
}