using Bookly.Domain.Entities.Bookings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookly.Infrastructure.Persistence.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Reference)
            .IsRequired()
            .HasMaxLength(11);

        builder.HasIndex(b => b.Reference)
            .IsUnique();

        builder.Property(b => b.UserId)
            .IsRequired();

        builder.Property(b => b.HotelId)
            .IsRequired();

        builder.Property(b => b.GuestName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(b => b.CheckIn)
            .IsRequired();

        builder.Property(b => b.CheckOut)
            .IsRequired();

        builder.Property(b => b.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(b => b.TotalPrice)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.HasQueryFilter(b => b.DeletedAt == null);

        builder.HasMany(b => b.BookingRooms)
            .WithOne()
            .HasForeignKey(br => br.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(b => b.BookingRooms)
            .HasField("_bookingRooms")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
