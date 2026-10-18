using Bookly.Domain.Entities.Amenities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookly.Infrastructure.Persistence.Configurations;

public class AmenityCategoryConfiguration : IEntityTypeConfiguration<AmenityCategory>
{
    public void Configure(EntityTypeBuilder<AmenityCategory> builder)
    {
        builder.HasIndex(c => c.NameEn).IsUnique();
        builder.HasIndex(c => c.NameAr).IsUnique();
    }
}
