using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class WeatherDataFetcher : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(1);
    private readonly ILogger<WeatherSyncService> _logger;

    public WeatherDataFetcher(IServiceScopeFactory scopeFactory, ILogger<WeatherSyncService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var syncService = scope.ServiceProvider.GetRequiredService<IWeatherSyncService>();
                try
                {
                    await syncService.SyncAllStationsAsync();

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Weather sync failed");
                }
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }
}