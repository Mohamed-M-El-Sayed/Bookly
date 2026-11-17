using Bookly.Domain.Entities.Cancellation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookly.Infrastructure.Persistence.Configurations;

public class CancellationRuleConfiguration : IEntityTypeConfiguration<CancellationRule>
{
    public void Configure(EntityTypeBuilder<CancellationRule> builder)
    {
        builder.ToTable("CancellationRules", table =>
        {
            table.HasCheckConstraint(
                "CK_CancellationRules_DaysBeforeCheckIn",
                "[DaysBeforeCheckIn] >= 0");

            table.HasCheckConstraint(
                "CK_CancellationRules_RefundPercentage",
                "[RefundPercentage] >= 0 AND [RefundPercentage] <= 100");
        });
        builder.Property(cr => cr.RefundPercentage)
            .HasPrecision(5, 2);
        builder.HasOne(cr => cr.CancellationPolicy)
            .WithMany(cp => cp.Rules)
            .HasForeignKey(cr => cr.CancellationPolicyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(cr => new
        {
            cr.CancellationPolicyId,
            cr.DaysBeforeCheckIn
        }).IsUnique();

    }
}
