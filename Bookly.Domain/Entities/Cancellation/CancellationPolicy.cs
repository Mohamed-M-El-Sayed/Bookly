using Bookly.Domain.Entities.Base;
using Bookly.Domain.Entities.Hotels;

namespace Bookly.Domain.Entities.Cancellation;

public class CancellationPolicy : BaseEntity
{
    public int HotelId { get; private set; }
    public Hotel Hotel { get; private set; } = null!;
    public ICollection<CancellationRule> Rules { get; private set; } = [];
    private CancellationPolicy() { }
}
