using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MiniCrm.Application.Common;

namespace MiniCrm.Api.Hubs;

// Genel amaçlı bildirim hub'ı: destek sohbeti (Faz 1) ve ileride sipariş
// durumu gibi başka "bir şey değişti, ilgili tarafı yenile" senaryoları
// için tek bir bağlantı üzerinden farklı event adlarıyla kullanılabilir.
// Client'tan çağrılabilir bir method yoktur; yalnızca sunucu → client push.
[Authorize]
public class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        if (Context.User?.IsInRole(AppRoles.Admin) == true)
        {
            await Groups.AddToGroupAsync(
                Context.ConnectionId,
                "admins");
        }
        else
        {
            var userId =
                Context.User?.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(
                    Context.ConnectionId,
                    $"customer-{userId}");
            }
        }

        await base.OnConnectedAsync();
    }
}
