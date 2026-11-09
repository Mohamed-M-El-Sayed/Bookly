using Bookly.Domain.Entities.Cancellation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookly.Infrastructure.Persistence.Configurations
{
    public class BookingCancellationRuleConfiguration : IEntityTypeConfiguration<BookingCancellationRule>
    {
        public void Configure(EntityTypeBuilder<BookingCancellationRule> builder)
        {
            builder.ToTable("BookingCancellationRules", table =>
            {
                table.HasCheckConstraint(
                    "CK_BookingCancellationRules_DaysBeforeCheckIn",
                    "[DaysBeforeCheckIn] >= 0");

                table.HasCheckConstraint(
                    "CK_BookingCancellationRules_RefundPercentage",
                    "[RefundPercentage] >= 0 AND [RefundPercentage] <= 100");
            });
            builder.Property(x => x.RefundPercentage)
                        .HasPrecision(5, 2);

            builder.HasOne(x => x.Booking)
                .WithMany(x => x.CancellationRules)
                .HasForeignKey(x => x.BookingId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
