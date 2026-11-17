using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.Cancellation;

namespace Bookly.Application.Features.CancellationPolicies.Specifications;

public class CancellationRuleByIdSpecification : BaseSpecification<CancellationRule>
{
    public CancellationRuleByIdSpecification(int hotelId, int ruleId)
    {
        Criteria = x =>
            x.Id == ruleId &&
            x.CancellationPolicy.HotelId == hotelId;
    }
}
