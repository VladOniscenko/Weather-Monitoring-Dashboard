using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Weather.Domain.Entities;

public class WeatherStationConfiguration : IEntityTypeConfiguration<WeatherStation>
{
    public void Configure(EntityTypeBuilder<WeatherStation> entity)
    {
        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(255);

        entity.Property(e => e.Latitude)
            .HasPrecision(9, 6);

        entity.Property(e => e.Longitude)
            .HasPrecision(9, 6);

        entity.HasOne(s => s.City)
            .WithMany()
            .HasForeignKey(c => c.CityId);

        entity.HasMany(s => s.Readings)
            .WithOne(wr => wr.Station)
            .HasForeignKey(s => s.StationId);
        
        entity.Property(s => s.LastSyncedAt)
            .HasDefaultValueSql("NOW()");
    }
}