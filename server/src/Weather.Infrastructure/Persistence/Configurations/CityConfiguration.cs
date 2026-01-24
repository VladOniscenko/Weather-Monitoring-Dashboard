using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Weather.Domain.Entities;

public class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> entity)
    {

        entity.Property(c => c.Name).HasMaxLength(100).IsRequired();
        entity.Property(c => c.Timezone).HasMaxLength(50).IsRequired();
        entity.Property(c => c.Latitude).IsRequired();
        entity.Property(c => c.Longitude).IsRequired();
        entity.Property(c => c.Population).HasDefaultValue(0);
        entity.Property(c => c.Timezone).HasMaxLength(100);

        entity.HasOne(city => city.Country)
            .WithMany(c => c.Cities)
            .HasForeignKey(city => city.CountryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}