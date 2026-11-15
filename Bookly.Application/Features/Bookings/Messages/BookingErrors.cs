using Bookly.Application.Common.Results;

namespace Bookly.Application.Features.Bookings.Messages;

public static class BookingErrors
{
    public static Error NotFound() =>
    ErrorFactory.NotFound("Booking.NotFound",
        "The requested booking was not found.");
    public static Error ExceedsMaxOccupancy() =>
       ErrorFactory.Validation(
           "Booking.ExceedsMaxOccupancy",
           "The number of guests exceeds the room's maximum occupancy.");
    public static Error RoomNotAvailable() =>
      ErrorFactory.Conflict("Booking.RoomNotAvailable",
          "The requested room is not available for the selected dates.");
    public static Error AccessDenied() =>
        ErrorFactory.Forbidden("Booking.AccessDenied",
            "You are not allowed to access this booking.");
    public static Error InvalidStatus() =>
        ErrorFactory.Conflict("Booking.InvalidStatus",
            "Booking status does not allow this operation.");
    public static Error Expired() =>
        ErrorFactory.Conflict("Booking.Expired",
            "Booking has expired.");
    public static Error InvalidBoardOption() =>
        ErrorFactory.Validation("Booking.InvalidBoardOption",
        "The selected board option is not available for this room type.");
    public static Error CannotBeCancelled() =>
        ErrorFactory.Conflict("booking.cannotBeCancelled",
        "This booking cannot be cancelled.");
    public static Error CancellationAlreadyPending() =>
        ErrorFactory.Conflict("Booking.CancellationAlreadyPending",
        "A cancellation request is already pending for this booking.");

    public static Error AlreadyCancelled() =>
         ErrorFactory.Conflict("Booking.AlreadyCancelled",
            "This booking has already been cancelled.");

}
