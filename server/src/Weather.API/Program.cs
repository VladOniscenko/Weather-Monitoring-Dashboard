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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
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

app.MapControllers();
app.UseHttpsRedirection();
app.UseCors("AllowReactApp");
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<GlobalExceptionMiddleware>();

// --- TRIGGER SEEDER START ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // 1. Get the DbContext
        var context = services.GetRequiredService<AppDbContext>();

        // 2. Optional: Automatically run migrations on startup
        // This is helpful in Docker so you don't have to run 'ef database update' manually
        await context.Database.MigrateAsync();

        // 3. Call your static Seeder method
        await DbSeeder.SeedCountriesAndCities(context);
        await DbSeeder.SeedWeatherStations(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or seeding the database.");
    }
}

app.Run();