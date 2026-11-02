using Bookly.Application.Common.Results;

namespace Bookly.Application.Features.Reviews.Errors;

public static class ReviewErrors
{
    public static Error BookingNotFound() =>
        ErrorFactory.NotFound("Review.BookingNotFound", "The specified booking was not found.");

    public static Error UnauthorizedBooking() =>
        ErrorFactory.Unauthorized("Review.UnauthorizedBooking", "You do not have permission to review this booking. The booking must belong to your account.");
    public static Error BookingNotCompleted() =>
    ErrorFactory.Conflict("Review.BookingNotCompleted", "Booking must be completed before submitting a review.");
    public static Error AlreadyReviewed() =>
        ErrorFactory.Conflict("Review.AlreadyReviewed", "A review for this booking already exists.");
}
