using Bookly.Domain.Entities.RoomTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookly.Infrastructure.Persistence.Configurations;

public class RoomAvailabilityConfiguration : IEntityTypeConfiguration<RoomAvailability>
{
    public void Configure(EntityTypeBuilder<RoomAvailability> builder)
    {
        builder.HasKey(x => new { x.RoomTypeId, x.Date });

        builder.Property<byte[]>("RowVersion")
            .IsRowVersion();

        builder.ToTable(t => t.HasCheckConstraint(
           "CK_RoomAvailability_Balance",
           "[TotalRooms] = [AvailableRooms] + [ReservedRooms]"));
    }
}
