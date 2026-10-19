using Bookly.Domain.Entities.Hotels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookly.Infrastructure.Persistence.Configurations;

internal class HotelAmenityConfiguration : IEntityTypeConfiguration<HotelAmenity>
{
    public void Configure(EntityTypeBuilder<HotelAmenity> builder)
    {
        builder.HasKey(x => new { x.HotelId, x.AmenityId });
        // Deleting a hotel will remove its link 
        builder.HasOne(x => x.Hotel)
                  .WithMany(h => h.Amenities)
                  .HasForeignKey(x => x.HotelId)
                  .OnDelete(DeleteBehavior.Cascade);
        // An amenity that is in use cannot be deleted
        builder.HasOne(x => x.Amenity)
            .WithMany()
            .HasForeignKey(x => x.AmenityId)
            .OnDelete(DeleteBehavior.Restrict);

        // The search filter looks up hotels by amenity
        builder.HasIndex(x => x.AmenityId);
    }
}
