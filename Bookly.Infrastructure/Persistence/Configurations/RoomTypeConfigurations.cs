using Bookly.Domain.Entities.RoomTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookly.Infrastructure.Persistence.Configurations
{
    public class RoomTypeConfigurations : IEntityTypeConfiguration<RoomType>
    {
        public void Configure(EntityTypeBuilder<RoomType> builder)
        {
            builder.HasQueryFilter(x => x.DeletedAt == null);
            builder.Property(r => r.BasePricePerNight)
                .HasPrecision(18, 2)
                .IsRequired();
            builder.Property(x => x.SizeSqm)
                .HasPrecision(5, 2)
                .IsRequired();
            builder.Navigation(h => h.Amenities)
              .HasField("_roomTypeAmenities")
              .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
