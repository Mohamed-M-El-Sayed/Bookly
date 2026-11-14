using FluentValidation;

namespace Bookly.Application.Features.BoardOptions.Commands.UpdateBoardOption;

public class UpdateBoardOptionCommandValidator : AbstractValidator<UpdateBoardOptionCommand>
{
    public UpdateBoardOptionCommandValidator()
    {
        RuleFor(x => x.RoomTypeId)
            .GreaterThan(0)
            .WithErrorCode("Validation.RoomTypeId.Invalid")
            .WithMessage("A valid room type id is required.");

        RuleFor(x => x.BoardOptionId)
            .GreaterThan(0)
            .WithErrorCode("Validation.BoardOption.Id.Invalid")
            .WithMessage("A valid board option id is required.");

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithErrorCode("Validation.BoardOption.Type.Invalid")
            .WithMessage("A valid board type is required.");

        RuleFor(x => x.ExtraPricePerNight)
            .GreaterThanOrEqualTo(0)
            .WithErrorCode("Validation.BoardOption.ExtraPricePerNight.Negative")
            .WithMessage("Extra price per night cannot be negative.");
    }
}
