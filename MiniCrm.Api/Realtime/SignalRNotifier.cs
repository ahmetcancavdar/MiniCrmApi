using Microsoft.AspNetCore.SignalR;
using MiniCrm.Api.Hubs;
using MiniCrm.Application.Interfaces.Services;

namespace MiniCrm.Api.Realtime;

public sealed class SignalRNotifier : IRealtimeNotifier
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public SignalRNotifier(
        IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public Task NotifyAdminsAsync(
        string eventName,
        object payload,
        CancellationToken cancellationToken = default)
    {
        return _hubContext.Clients
            .Group("admins")
            .SendAsync(
                eventName,
                payload,
                cancellationToken);
    }

    public Task NotifyCustomerAsync(
        Guid customerUserId,
        string eventName,
        object payload,
        CancellationToken cancellationToken = default)
    {
        return _hubContext.Clients
            .Group($"customer-{customerUserId}")
            .SendAsync(
                eventName,
                payload,
                cancellationToken);
    }

    public Task NotifyAllAsync(
        string eventName,
        object payload,
        CancellationToken cancellationToken = default)
    {
        return _hubContext.Clients
            .All
            .SendAsync(
                eventName,
                payload,
                cancellationToken);
    }
}
