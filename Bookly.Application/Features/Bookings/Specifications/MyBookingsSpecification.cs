using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.Bookings;
using Bookly.Domain.Enums;

namespace Bookly.Application.Features.Bookings.Specifications;

public class MyBookingsSpecification : BaseSpecification<Booking>
{
    public MyBookingsSpecification(Guid userId, int pageIndex, int PageSize, BookingStatus? status)
    {
        Criteria = b => b.UserId == userId
        && (status == null || b.Status == status);
        AddInclude(b => b.BookingRooms);
        ApplyPagination(pageIndex, PageSize);
        ApplyOrderByDescending(b => b.CreatedAt);
    }
}
