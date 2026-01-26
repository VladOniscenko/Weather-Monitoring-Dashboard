using Microsoft.Extensions.DependencyInjection;
using Weather.Application.Common.Interfaces;
using Weather.Application.Services;

namespace Weather.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register Business Services
        services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
        services.AddScoped<IWeatherStationService, WeatherStationService>();
        services.AddScoped<IWeatherReadingService, WeatherReadingService>();
        services.AddScoped<IWeatherSyncService, WeatherSyncService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICityService, CityService>();
        services.AddScoped<ICountryService, CountryService>();

        return services;
    }
}