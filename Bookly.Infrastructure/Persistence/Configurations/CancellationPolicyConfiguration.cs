using Bookly.Domain.Entities.Cancellation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookly.Infrastructure.Persistence.Configurations;

public class CancellationPolicyConfiguration : IEntityTypeConfiguration<CancellationPolicy>
{
    public void Configure(EntityTypeBuilder<CancellationPolicy> builder)
    {

        builder.HasOne(cp => cp.Hotel)
            .WithOne(h => h.CancellationPolicy)
            .HasForeignKey<CancellationPolicy>(cp => cp.HotelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(cp => cp.HotelId)
            .IsUnique();
    }
}
