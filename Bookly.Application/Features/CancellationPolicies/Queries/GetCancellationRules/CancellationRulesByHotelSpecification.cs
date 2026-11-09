using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.Cancellation;

namespace Bookly.Application.Features.CancellationPolicies.Queries.GetCancellationRules;

public class CancellationRulesByHotelSpecification : BaseSpecification<CancellationRule>
{
    public CancellationRulesByHotelSpecification(int hotelId)
    {
        Criteria = rule => rule.CancellationPolicy.HotelId == hotelId;
    }
}
