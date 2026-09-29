using FluentValidation;

namespace Bookly.Application.Features.SeasonalPricing.Commands.CreateSeasonalPrice;

public class CreateSeasonalPriceCommandValidator : AbstractValidator<CreateSeasonalPriceCommand>
{
    public CreateSeasonalPriceCommandValidator()
    {
        RuleFor(x => x.RoomTypeId)
           .GreaterThan(0).WithMessage("A valid room type id is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required.")
            .GreaterThanOrEqualTo(x => x.StartDate)
                .WithMessage("End date must be after or equal to start date.");

        RuleFor(x => x.PricePerNight)
            .GreaterThan(0).WithMessage("Price per night must be greater than zero.");
    }
}
