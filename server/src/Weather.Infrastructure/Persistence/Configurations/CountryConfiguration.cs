using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Weather.Domain.Entities;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> entity)
    {

        entity.Property(c => c.Name).HasMaxLength(100).IsRequired();
        entity.Property(c => c.CCA2).HasMaxLength(2).IsRequired();
        entity.Property(c => c.CCA3).HasMaxLength(3).IsRequired();
        entity.Property(c => c.Region).HasMaxLength(50);
        entity.Property(c => c.Subregion).HasMaxLength(50);
        entity.Property(c => c.Capital).HasMaxLength(50);
        entity.Property(c => c.Flag).HasMaxLength(10);
        entity.Property(e => e.Capital).HasMaxLength(100);
        entity.Property(e => e.Latitude).IsRequired();
        entity.Property(e => e.Longitude).IsRequired();
        entity.Property(e => e.Independent).IsRequired();
        entity.Property(e => e.Landlocked).IsRequired();
        entity.Property(e => e.Flag).HasMaxLength(10);
        entity.HasIndex(e => e.CCA2).IsUnique();
        entity.HasIndex(e => e.CCA3).IsUnique();
        
        entity.HasMany(country => country.Cities)
            .WithOne(s => s.Country)
            .HasForeignKey(s => s.CountryId);
    }
}