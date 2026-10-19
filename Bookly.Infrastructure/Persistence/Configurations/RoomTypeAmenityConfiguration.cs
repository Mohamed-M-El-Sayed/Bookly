using Bookly.Domain.Entities.RoomTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookly.Infrastructure.Persistence.Configurations
{
    public class RoomTypeAmenityConfiguration : IEntityTypeConfiguration<RoomTypeAmenity>
    {
        public void Configure(EntityTypeBuilder<RoomTypeAmenity> builder)
        {
            builder.HasKey(x => new { x.RoomTypeId, x.AmenityId });

            builder.HasOne(x => x.RoomType)
                .WithMany(r => r.Amenities)
                .HasForeignKey(x => x.RoomTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Amenity)
                .WithMany()
                .HasForeignKey(x => x.AmenityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.AmenityId);
        }
    }
}
