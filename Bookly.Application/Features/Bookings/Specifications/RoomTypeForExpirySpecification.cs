using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.RoomTypes;

namespace Bookly.Application.Features.Bookings.Specifications;

internal class RoomTypeForExpirySpecification : BaseSpecification<RoomType>
{
    public RoomTypeForExpirySpecification(int roomTypeId, DateOnly checkIn, DateOnly checkOut)
    {
        Criteria = r => r.Id == roomTypeId;
        AddInclude(r => r.RoomAvailabilities.Where(a => a.Date >= checkIn && a.Date < checkOut));
    }
}
