using Microsoft.EntityFrameworkCore;
using Weather.Domain.Entities;
using Weather.Domain.Enums;

namespace Weather.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    // The constructor is required for Dependency Injection in Program.cs
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Tables
    public DbSet<User> Users => Set<User>();
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<City> Cities => Set<City>();
    public DbSet<WeatherStation> WeatherStations => Set<WeatherStation>();
    public DbSet<WeatherReading> WeatherReadings => Set<WeatherReading>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                // Id is primary key
                modelBuilder.Entity(entityType.ClrType)
                    .HasKey(nameof(BaseEntity.Id));

                // IsDeleted default false
                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(BaseEntity.IsDeleted))
                    .HasDefaultValue(false);

                // CreatedAt default UTC now
                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(BaseEntity.CreatedAt))
                    .HasDefaultValueSql("NOW()");
            }
        }
    }
}