using Bookly.Domain.Entities.Bookings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookly.Infrastructure.Persistence.Configurations
{
    internal class BookingRoomConfiguration
    {
        public void Configure(EntityTypeBuilder<BookingRoom> builder)
        {
            builder.HasKey(br => br.Id);

            builder.Property(br => br.BookingId)
                .IsRequired();

            builder.Property(br => br.RoomTypeId)
                .IsRequired();

            builder.Property(br => br.Guests)
                .IsRequired();

            builder.Property(br => br.PriceAtBooking)
                .HasPrecision(18, 2)
                .IsRequired();
            builder.HasQueryFilter(b => b.DeletedAt == null);
            builder.HasOne<Booking>()
                .WithMany(b => b.BookingRooms)
                .HasForeignKey(br => br.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(br => br.RoomType)
                .WithMany()
                .HasForeignKey(br => br.RoomTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
