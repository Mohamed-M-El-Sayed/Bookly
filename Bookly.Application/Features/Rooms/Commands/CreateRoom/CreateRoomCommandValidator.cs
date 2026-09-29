using FluentValidation;

namespace Bookly.Application.Features.Rooms.Commands.CreateRoom;

public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
{
    public CreateRoomCommandValidator()
    {
        RuleFor(x => x.RoomTypeId)
            .GreaterThan(0)
            .WithMessage("Room type is required.");
        RuleFor(x => x.RoomNumber)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("Room number is required and must not exceed 50 characters.");
    }
}
