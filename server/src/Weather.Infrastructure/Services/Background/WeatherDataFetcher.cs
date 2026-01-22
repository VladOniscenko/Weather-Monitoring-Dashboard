using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class WeatherDataFetcher : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(1);

    public WeatherDataFetcher(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var syncService = scope.ServiceProvider.GetRequiredService<IWeatherSyncService>();
                await syncService.SyncAllStationsAsync();
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }
}