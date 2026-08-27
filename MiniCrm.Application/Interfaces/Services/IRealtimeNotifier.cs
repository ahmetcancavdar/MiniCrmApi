namespace MiniCrm.Application.Interfaces.Services;

// Application/Domain katmanları SignalR paketine bağımlı olmasın diye,
// gerçek zamanlı bildirim gönderimi bu soyutlama üzerinden yapılır.
// Somut implementasyon (IHubContext kullanan) MiniCrm.Api katmanında yaşar
// (IEmailService'in Infrastructure'da somutlaşması ile aynı desen).
public interface IRealtimeNotifier
{
    Task NotifyAdminsAsync(
        string eventName,
        object payload,
        CancellationToken cancellationToken = default);

    Task NotifyCustomerAsync(
        Guid customerUserId,
        string eventName,
        object payload,
        CancellationToken cancellationToken = default);

    // Herkese (bağlı tüm admin + customer istemcilerine) yayın yapılır;
    // ürün kataloğu/stok gibi zaten herkese açık verilerdeki değişiklikler
    // için kullanılır.
    Task NotifyAllAsync(
        string eventName,
        object payload,
        CancellationToken cancellationToken = default);
}
