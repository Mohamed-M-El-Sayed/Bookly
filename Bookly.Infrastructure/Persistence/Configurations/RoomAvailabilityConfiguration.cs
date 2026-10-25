using Bookly.Domain.Entities.RoomTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookly.Infrastructure.Persistence.Configurations;

public class RoomAvailabilityConfiguration : IEntityTypeConfiguration<RoomAvailability>
{
    public void Configure(EntityTypeBuilder<RoomAvailability> builder)
    {
        builder.HasKey(x => new { x.RoomTypeId, x.Date });

        builder.Ignore(x => x.AvailableRooms);

        builder.Property(x => x.Date)
            .HasColumnType("date");

        builder.Property<byte[]>("RowVersion")
            .IsRowVersion();

        builder.ToTable(t => t.HasCheckConstraint(
            "CK_RoomAvailability_Balance",
            "[TotalRooms] >= 0 AND " +
            "[ReservedRooms] >= 0 AND " +
            "[ReservedRooms] <= [TotalRooms]"));

        builder.HasOne(x => x.RoomType)
          .WithMany(x => x.RoomAvailabilities)
          .HasForeignKey(x => x.RoomTypeId)
          .OnDelete(DeleteBehavior.Cascade);
    }
}