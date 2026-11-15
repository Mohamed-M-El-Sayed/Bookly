using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.SeasonalPricing;

namespace Bookly.Application.Features.SeasonalPricing.Specifications;

public class SeasonalPricesByRoomTypeIdSpecification : BaseSpecification<SeasonalPrice>
{
    public SeasonalPricesByRoomTypeIdSpecification(int roomTypeId, int pageNumber, int pageSize)
    {
        Criteria = x => x.RoomTypeId == roomTypeId;
        ApplyOrderBy(x => x.StartDate);
        ApplyPagination(pageNumber, pageSize);
    }
}
