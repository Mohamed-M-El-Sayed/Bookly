using Bookly.Domain.Entities.Amenities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookly.Infrastructure.Persistence.Configurations;

public class AmenityConfiguration : IEntityTypeConfiguration<Amenity>
{
    public void Configure(EntityTypeBuilder<Amenity> builder)
    {
        builder.Property(a => a.Scope)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();
        builder.HasIndex(a => a.NameEn).IsUnique();
        builder.HasIndex(a => a.NameAr).IsUnique();

        // A category that still has amenities cannot be deleted
        builder.HasOne(a => a.Category)
             .WithMany(c => c.Amenities)
             .HasForeignKey(a => a.CategoryId)
             .OnDelete(DeleteBehavior.Restrict);
    }
}
