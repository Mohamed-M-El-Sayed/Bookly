using FluentValidation;

namespace Bookly.Application.Features.Reviews.Commands.UpdateReview;

public class UpdateReviewCommandValidator : AbstractValidator<UpdateReviewCommand>
{
    public UpdateReviewCommandValidator()
    {
        RuleFor(x => x.ReviewId)
            .GreaterThan(0)
            .WithErrorCode("Validation.Review.InvalidReviewId")
            .WithMessage("Review ID must be greater than 0.");

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