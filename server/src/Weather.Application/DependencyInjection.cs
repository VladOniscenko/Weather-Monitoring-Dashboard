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
        
        // If you use MediatR or FluentValidation, register them here
        // services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        services.AddScoped<IUserService, UserService>();
        services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
        services.AddScoped<CityService>();

        return services;
    }
}