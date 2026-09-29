using FluentValidation;

namespace Bookly.Application.Features.BoardOptions.Commands.CreateBoardOption;

public class CreateBoardOptionCommandValidator : AbstractValidator<CreateBoardOptionCommand>
{
    public CreateBoardOptionCommandValidator()
    {
        RuleFor(x => x.RoomTypeId)
            .GreaterThan(0).WithMessage("A valid room type id is required.");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("A valid board type is required.");

        RuleFor(x => x.ExtraPricePerNight)
            .GreaterThanOrEqualTo(0).WithMessage("Extra price per night cannot be negative.");
    }
}
