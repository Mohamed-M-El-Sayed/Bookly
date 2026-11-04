using FluentValidation;

namespace Bookly.Application.Features.Reviews.Queries.GetHotelReviews;

public class GetHotelReviewsQueryValidator : AbstractValidator<GetHotelReviewsQuery>
{
    public GetHotelReviewsQueryValidator()
    {
        RuleFor(x => x.HotelId)
            .GreaterThan(0)
            .WithErrorCode("Validation.Review.InvalidHotelId")
            .WithMessage("Hotel ID must be greater than 0.");

        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithErrorCode("Validation.Common.InvalidPageNumber")
            .WithMessage("Page number must be greater than 0.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(10, 50)
            .WithErrorCode("Validation.Common.InvalidPageSize")
            .WithMessage("Page size must be between 10 and 50.");
    }
}