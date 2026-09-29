using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.RoomTypes;

namespace Bookly.Application.Features.SeasonalPricing.Specifications;

public class RoomTypeWithSeasonalPricesSpecification : BaseSpecification<RoomType>
{
    public RoomTypeWithSeasonalPricesSpecification(int roomTypeId)
    {
        Criteria = x => x.Id == roomTypeId;
        AddInclude(x => x.SeasonalPrices);
    }

}
