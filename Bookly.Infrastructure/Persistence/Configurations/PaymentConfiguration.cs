using Bookly.Domain.Entities.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookly.Infrastructure.Persistence.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.Property(p => p.Amount)
             .HasPrecision(18, 2)
             .IsRequired();
        builder.Property(p => p.Status)
            .HasConversion<string>()
            .IsRequired();
        builder.Property(p => p.RefundAmount)
          .HasPrecision(18, 2);
        builder.HasQueryFilter(p => p.DeletedAt == null);
        builder.HasOne(p => p.Booking)
           .WithMany(b => b.Payments)
           .HasForeignKey(p => p.BookingId)
           .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(p => p.BookingId);
    }
}
