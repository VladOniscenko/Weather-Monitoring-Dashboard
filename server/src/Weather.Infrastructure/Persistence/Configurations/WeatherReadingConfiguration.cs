using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Weather.Domain.Entities;

public class WeatherReadingConfiguration : IEntityTypeConfiguration<WeatherReading>
{
    public void Configure(EntityTypeBuilder<WeatherReading> entity)
    {
        entity.HasOne(ws => ws.Station)
            .WithMany(s => s.Readings)
            .HasForeignKey(wr => wr.StationId);

        entity.Property(wr => wr.MainCondition)
            .HasMaxLength(50);

        entity.Property(wr => wr.Description)
            .HasMaxLength(50);

        entity.Property(wr => wr.Icon)
            .HasMaxLength(50);

        entity.Property(wr => wr.Temperature)
            .HasPrecision(9, 6);

        entity.Property(wr => wr.FeelsLike)
            .HasPrecision(9, 6);

        entity.Property(wr => wr.MinTemp)
            .HasPrecision(9, 6);

        entity.Property(wr => wr.MaxTemp)
            .HasPrecision(9, 6);

        entity.Property(wr => wr.Pressure)
            .HasDefaultValue(0);

        entity.Property(wr => wr.Humidity)
            .HasDefaultValue(0);

        entity.Property(wr => wr.SeaLevel)
            .HasDefaultValue(0);

        entity.Property(wr => wr.GroundLevel)
            .HasDefaultValue(0);

        entity.Property(wr => wr.Visibility)
            .HasDefaultValue(0);

        entity.Property(wr => wr.WindSpeed)
            .HasPrecision(9, 6);

        entity.Property(wr => wr.WindDeg)
            .HasDefaultValue(0);

        entity.Property(wr => wr.Cloudiness)
            .HasDefaultValue(0);

        entity.Property(e => e.Rain)
              .HasPrecision(9, 6)
              .HasDefaultValue(0.0);

        entity.Property(e => e.Snow)
              .HasPrecision(9, 6)
              .HasDefaultValue(0.0);

        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("NOW()");
    }
}