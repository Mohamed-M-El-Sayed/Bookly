using FluentValidation;

namespace Bookly.Application.Features.BoardOptions.Commands.CreateBoardOption;

public class CreateBoardOptionCommandValidator
    : AbstractValidator<CreateBoardOptionCommand>
{
    public CreateBoardOptionCommandValidator()
    {
        RuleFor(x => x.RoomTypeId)
            .GreaterThan(0)
                .WithErrorCode("Validation.RoomTypeId.Invalid")
                .WithMessage("A valid room type id is required.");

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