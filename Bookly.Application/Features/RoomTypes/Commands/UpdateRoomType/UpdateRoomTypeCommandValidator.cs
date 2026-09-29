using FluentValidation;

namespace Bookly.Application.Features.RoomTypes.Commands.UpdateRoomType;

public class UpdateRoomTypeCommandValidato : AbstractValidator<UpdateRoomTypeCommand>
{
    public UpdateRoomTypeCommandValidato()
    {
        RuleFor(x => x.HotelId)
            .GreaterThan(0);

        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Room type name is required.")
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(1000);

        RuleFor(x => x.BasePricePerNight)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleFor(x => x.MaxOccupancy)
            .GreaterThan(0).WithMessage("Max occupancy must be at least 1.")
            .LessThanOrEqualTo(10).WithMessage("Max occupancy seems unrealistic.");

        RuleFor(x => x.View)
            .NotEmpty().WithMessage("View is required.")
            .MaximumLength(50);
    }
}
