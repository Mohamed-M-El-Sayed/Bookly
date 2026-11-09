using Bookly.Domain.Entities.Bookings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookly.Infrastructure.Persistence.Configurations;

public class BookingCancellationConfiguration : IEntityTypeConfiguration<BookingCancellation>
{
    public void Configure(EntityTypeBuilder<BookingCancellation> builder)
    {
        builder.ToTable("BookingCancellations", table =>
        {
            table.HasCheckConstraint(
                "CK_BookingCancellations_CancellationFee",
                "[CancellationFee] >= 0");

            table.HasCheckConstraint(
                "CK_BookingCancellations_RefundAmount",
                "[RefundAmount] >= 0");
        });
        builder.Property(bc => bc.CancellationFee)
            .HasPrecision(18, 2);

        builder.Property(bc => bc.RefundAmount)
            .HasPrecision(18, 2);

        builder.HasOne(bc => bc.Booking)
            .WithOne(b => b.Cancellation)
            .HasForeignKey<BookingCancellation>(bc => bc.BookingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(bc => bc.BookingId)
            .IsUnique();
    }
}
