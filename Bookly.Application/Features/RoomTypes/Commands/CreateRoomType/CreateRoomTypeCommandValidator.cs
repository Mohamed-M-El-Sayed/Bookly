using FluentValidation;

namespace Bookly.Application.Features.RoomTypes.Commands.CreateRoomType;

public class CreateRoomTypeCommandValidator : AbstractValidator<CreateRoomTypeCommand>
{
    public CreateRoomTypeCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.BasePricePerNight).GreaterThan(0).WithMessage("Price must be greater than 0.");
        RuleFor(x => x.MaxOccupancy).GreaterThan(0).WithMessage("Max occupancy must be greater than 0.");
        RuleFor(x => x.View).NotEmpty().MaximumLength(100);
    }
}
