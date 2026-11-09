using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.Hotels;

namespace Bookly.Application.Features.CancellationPolicies.Specifications;

public class HotelCancellationRulesSpecification : BaseSpecification<Hotel>
{
    public HotelCancellationRulesSpecification(int hotelId)
    {
        Criteria = h => h.Id == hotelId;
        AddInclude(h => h.CancellationPolicy.Rules);
    }
}
