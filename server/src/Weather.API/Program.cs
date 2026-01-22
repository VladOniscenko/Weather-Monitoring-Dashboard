using Weather.Application;
using Weather.Infrastructure;
using Weather.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Weather.API.Middleware;
using Weather.Application.Common.Interfaces;
using Weather.Infrastructure.ExternalApi.OpenWeather;

var builder = WebApplication.CreateBuilder(args);

// --- REGISTRATION PHASE ---
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddSwaggerWithAuth();

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<GlobalExceptionMiddleware>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Open weather map client
builder.Services.AddHttpClient<IExternalWeatherApi, OpenWeatherApi>(client =>
{
    client.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
});

var app = builder.Build();

// --- EXECUTION PHASE ---

// 1. Automatic Migration Logic
// --- EXECUTION PHASE ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var context = services.GetRequiredService<AppDbContext>();

    // Attempt to migrate 10 times with a 3-second delay
    for (int i = 0; i < 10; i++)
    {
        try
        {
            logger.LogInformation("Database Automation: Checking migrations (Attempt {Attempt}/10)", i + 1);
            if ((await context.Database.GetPendingMigrationsAsync()).Any())
            {
                logger.LogInformation("Database Automation: Found pending migrations. Applying...");
                await context.Database.MigrateAsync();
                logger.LogInformation("Database Automation: Migration successful.");
            }
            break; // Exit loop on success
        }
        catch (Exception ex)
        {
            logger.LogWarning("Database Automation: Postgres not ready yet. Retrying in 3s...");
            await Task.Delay(3000);
            if (i == 9) throw new Exception("Database Automation: Failed to migrate after 10 attempts.", ex);
        }
    }
}

// 2. Middleware Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseMiddleware<GlobalExceptionMiddleware>();

app.Run();