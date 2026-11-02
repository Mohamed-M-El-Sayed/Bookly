using FluentValidation;

namespace Bookly.Application.Features.Reviews.Commands.CreateReview;

public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
{
    public CreateReviewCommandValidator()
    {
        RuleFor(x => x.BookingId)
            .GreaterThan(0)
            .WithErrorCode("Validation.Review.InvalidBookingId")
            .WithMessage("Booking ID must be greater than 0.");

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5)
            .WithErrorCode("Validation.Review.InvalidRating")
            .WithMessage("Rating must be between 1 and 5.");

        RuleFor(x => x.Comment)
            .MaximumLength(1000)
            .WithErrorCode("Validation.Review.InvalidComment")
            .WithMessage("Comment must not exceed 1000 characters.");
    }

}
