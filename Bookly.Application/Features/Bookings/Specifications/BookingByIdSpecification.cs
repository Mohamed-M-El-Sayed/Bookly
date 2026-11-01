using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.Bookings;

namespace Bookly.Application.Features.Bookings.Specifications;

public class BookingByIdSpecification : BaseSpecification<Booking>
{
    public BookingByIdSpecification(int bookingId, Guid userId)
    {
        Criteria = booking =>
            booking.Id == bookingId &&
            (
                booking.UserId == userId ||
                booking.Hotel.Managers.Any(m => m.UserId == userId)
            );

        AddInclude(b => b.BookingRooms);
        AddInclude(b => b.Hotel);
        AddInclude(b => b.Hotel.Images.Where(img => img.IsPrimary));
    }
}
