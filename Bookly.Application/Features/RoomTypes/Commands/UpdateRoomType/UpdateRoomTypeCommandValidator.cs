using FluentValidation;
namespace Bookly.Application.Features.RoomTypes.Commands.UpdateRoomType;

public class UpdateRoomTypeCommandValidator : AbstractValidator<UpdateRoomTypeCommand>
{
    public UpdateRoomTypeCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
        RuleFor(x => x.HotelId)
            .GreaterThan(0);
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Room type name is required.")
            .MaximumLength(100);
        RuleFor(x => x.NameAr)
            .NotEmpty().WithMessage("Arabic room type name is required.")
            .MaximumLength(100);
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(1000);
        RuleFor(x => x.DescriptionAr)
            .NotEmpty().WithMessage("Arabic description is required.")
            .MaximumLength(1000);
        RuleFor(x => x.BasePricePerNight)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");
        RuleFor(x => x.MaxOccupancy)
            .GreaterThan(0).WithMessage("Max occupancy must be at least 1.")
            .LessThanOrEqualTo(10).WithMessage("Max occupancy seems unrealistic.");
        RuleFor(x => x.View)
            .NotEmpty().WithMessage("View is required.")
            .MaximumLength(100);
        RuleFor(x => x.ViewAr)
            .NotEmpty().WithMessage("Arabic view is required.")
            .MaximumLength(100);
        RuleFor(x => x.BedCount)
            .GreaterThan(0).WithMessage("Bed count must be greater than zero.");
        RuleFor(x => x.SizeSqm)
            .GreaterThan(0).WithMessage("Room size must be greater than zero.");
    }
}
