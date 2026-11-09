using Bookly.Domain.Entities.Base;

namespace Bookly.Domain.Entities.Cancellation;

public class CancellationRule : BaseEntity
{
    public int CancellationPolicyId { get; private set; }
    public int DaysBeforeCheckIn { get; private set; }
    public decimal RefundPercentage { get; private set; }
    public CancellationPolicy CancellationPolicy { get; private set; } = null!;
    private CancellationRule() { }


}
