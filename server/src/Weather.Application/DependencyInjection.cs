using Microsoft.Extensions.DependencyInjection;
using Weather.Application.Common.Interfaces;
using Weather.Application.Services;

namespace Weather.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register Business Services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<CityService>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
        services.AddScoped<IWeatherSyncService, WeatherSyncService>();
        return services;
    }
}