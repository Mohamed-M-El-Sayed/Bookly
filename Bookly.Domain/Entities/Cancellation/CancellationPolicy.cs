using Bookly.Domain.Entities.Base;
using Bookly.Domain.Entities.Hotels;

namespace Bookly.Domain.Entities.Cancellation;

public class CancellationPolicy : BaseEntity
{
    public int HotelId { get; private set; }
    public Hotel Hotel { get; private set; } = null!;
    private readonly List<CancellationRule> _rules = new();
    public IReadOnlyCollection<CancellationRule> Rules => _rules.AsReadOnly();
    private CancellationPolicy() { }
    private CancellationPolicy(int hotelId)
    {
        HotelId = hotelId;
    }
    public static CancellationPolicy Create(int hotelId)
    {
        if (hotelId <= 0)
            throw new ArgumentException("Hotel ID must be greater than zero.");

        return new CancellationPolicy(hotelId);
    }
    public void AddRules(IEnumerable<(int DaysBeforeCheckIn, decimal RefundPercentage)> rules)
    {
        ArgumentNullException.ThrowIfNull(rules);

        foreach (var rule in rules)
        {
            if (rule.DaysBeforeCheckIn < 0)
                throw new ArgumentException("Days before check-in cannot be negative.");
            if (rule.RefundPercentage is < 0 or > 100)
                throw new ArgumentException("Refund percentage must be between 0 and 100.");
            var alreadyExists = _rules.Any(x =>
                x.DaysBeforeCheckIn == rule.DaysBeforeCheckIn);
            if (alreadyExists)
                continue;
            _rules.Add(CancellationRule.Create(rule.DaysBeforeCheckIn, rule.RefundPercentage));
        }
    }
}
