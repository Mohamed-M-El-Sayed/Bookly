using Bookly.Application.Common.Specifications;
using Bookly.Domain.Entities.Bookings;

namespace Bookly.Application.Features.Bookings.Specifications;

public class BookingCancellationByBookingIdSpecification
    : BaseSpecification<BookingCancellation>
{
    public BookingCancellationByBookingIdSpecification(int bookingId)
    {
        Criteria = cancellation => cancellation.BookingId == bookingId;
    }
}