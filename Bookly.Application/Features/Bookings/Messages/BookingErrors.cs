using Bookly.Application.Common.Results;

namespace Bookly.Application.Features.Bookings.Messages;

public static class BookingErrors
{
    public static Error ExceedsMaxOccupancy() =>
       ErrorFactory.Validation(
           "Booking.ExceedsMaxOccupancy",
           "The number of guests exceeds the room's maximum occupancy.");
    //public static Error PendingBookingExists() =>
    //  ErrorFactory.Conflict(
    //      "Booking.PendingBookingExists",
    //      "You already have a pending booking for this hotel and dates.");
    public static Error RoomNotAvailable() =>
      ErrorFactory.Conflict(
          "Booking.RoomNotAvailable",
          "The requested room is not available for the selected dates.");
}
