using FluentValidation;

namespace Bookly.Application.Features.SeasonalPricing.Queries.GetSeasonalPricesByRoomTypeId;

public class GetSeasonalPricesByRoomTypeIdQueryValidator : AbstractValidator<GetSeasonalPricesByRoomTypeIdQuery>
{
    public GetSeasonalPricesByRoomTypeIdQueryValidator()
    {
        RuleFor(x => x.RoomTypeId)
            .GreaterThan(0)
            .WithErrorCode("Validation.SeasonalPrice.RoomTypeId.Invalid")
            .WithMessage("Room type ID must be greater than zero.");

        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithErrorCode("Validation.SeasonalPrice.PageNumber.Invalid")
            .WithMessage("Page number must be greater than zero.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(30, 50)
            .WithErrorCode("Validation.SeasonalPrice.PageSize.Invalid")
            .WithMessage("Page size must be between 30 and 50.");
    }
}