using FluentValidation;

namespace Bookly.Application.Features.SeasonalPricing.Commands.CreateSeasonalPrice;

public class CreateSeasonalPriceCommandValidator
    : AbstractValidator<CreateSeasonalPriceCommand>
{
    public CreateSeasonalPriceCommandValidator()
    {
        RuleFor(x => x.RoomTypeId)
            .GreaterThan(0)
            .WithErrorCode("Validation.SeasonalPrice.RoomTypeId.Invalid")
            .WithMessage("A valid room type id is required.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithErrorCode("Validation.SeasonalPrice.Name.Required")
            .WithMessage("Name is required.")
            .MaximumLength(100)
            .WithErrorCode("Validation.SeasonalPrice.Name.TooLong")
            .WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithErrorCode("Validation.SeasonalPrice.StartDate.Required")
            .WithMessage("Start date is required.");

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithErrorCode("Validation.SeasonalPrice.EndDate.Required")
            .WithMessage("End date is required.")
            .GreaterThanOrEqualTo(x => x.StartDate)
            .WithErrorCode("Validation.SeasonalPrice.EndDate.Invalid")
            .WithMessage("End date must be after or equal to start date.");

        RuleFor(x => x.PricePerNight)
            .GreaterThan(0)
            .WithErrorCode("Validation.SeasonalPrice.PricePerNight.Invalid")
            .WithMessage("Price per night must be greater than zero.");
    }
}