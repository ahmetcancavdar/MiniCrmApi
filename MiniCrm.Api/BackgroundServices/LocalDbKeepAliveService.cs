using Microsoft.EntityFrameworkCore;
using MiniCrm.Persistence.Context;

namespace MiniCrm.Api.BackgroundServices;

// LocalDB, bir süre kullanılmayınca kendini otomatik kapatıyor
// ("RANU instance is terminating in response to its internal time out"),
// ve bir sonraki bağlantıda ara sıra "SQL Server process failed to start"
// hatasıyla başarısız oluyor. Bu servis API ayakta olduğu sürece
// veritabanına düzenli aralıklarla hafif bir sorgu göndererek LocalDB'nin
// boşta kalıp kapanmasını engeller.
public sealed class LocalDbKeepAliveService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<LocalDbKeepAliveService> _logger;

    public LocalDbKeepAliveService(
        IServiceScopeFactory scopeFactory,
        ILogger<LocalDbKeepAliveService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Daha önce 2 dakikaydı; ama LocalDB'nin tekil veritabanı
        // ("MiniCrmDb") bazı ortamlarda bundan çok daha kısa bir sürede
        // (~1 dakika) boşta kalıp otomatik kapanabiliyor (error.log'da
        // tekrarlanan "Starting up database" olayları ile doğrulandı).
        // 2 dakikalık aralık bu durumda hiç koruma sağlamıyordu; 30
        // saniyeye düşürüldü.
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(30));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();

                var context = scope.ServiceProvider
                    .GetRequiredService<AppDbContext>();

                await context.Database.ExecuteSqlRawAsync(
                    "SELECT 1",
                    stoppingToken);
            }
            catch (Exception exception)
            {
                _logger.LogWarning(
                    exception,
                    "LocalDB keep-alive sorgusu başarısız oldu.");
            }
        }
    }
}
