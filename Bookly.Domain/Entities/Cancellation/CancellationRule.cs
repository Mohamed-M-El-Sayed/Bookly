using Bookly.Domain.Entities.Base;

namespace Bookly.Domain.Entities.Cancellation;

public class CancellationRule : BaseEntity
{
    public int CancellationPolicyId { get; private set; }
    public int DaysBeforeCheckIn { get; private set; }
    public decimal RefundPercentage { get; private set; }
    public CancellationPolicy CancellationPolicy { get; private set; } = null!;
    private CancellationRule() { }
    private CancellationRule(int daysBeforeCheckIn, decimal refundPercentage)
    {
        DaysBeforeCheckIn = daysBeforeCheckIn;
        RefundPercentage = refundPercentage;
    }
    public static CancellationRule Create(int daysBeforeCheckIn, decimal refundPercentage)
    {
        return new CancellationRule(daysBeforeCheckIn, refundPercentage);
    }
    public void Update(int daysBeforeCheckIn, decimal refundPercentage)
    {
        if (daysBeforeCheckIn < 0)
            throw new ArgumentException("Days before check-in cannot be negative.");

        if (refundPercentage is < 0 or > 100)
            throw new ArgumentException("Refund percentage must be between 0 and 100.");

        DaysBeforeCheckIn = daysBeforeCheckIn;
        RefundPercentage = refundPercentage;
    }
}
