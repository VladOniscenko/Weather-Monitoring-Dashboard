using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Weather.Domain.Enums;

using Weather.Domain.Entities;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.Property(e => e.Email)
              .IsRequired()
              .HasMaxLength(255);

        entity.HasIndex(e => e.Email)
              .IsUnique();

        entity.Property(e => e.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        entity.Property(e => e.Role)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(20);
    }
}