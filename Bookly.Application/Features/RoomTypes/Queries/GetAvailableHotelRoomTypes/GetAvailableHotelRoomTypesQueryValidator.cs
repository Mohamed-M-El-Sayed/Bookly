using FluentValidation;

namespace Bookly.Application.Features.RoomTypes.Queries.GetAvailableHotelRoomTypes;

public class GetAvailableHotelRoomTypesQueryValidator
    : AbstractValidator<GetAvailableHotelRoomTypesQuery>
{
    public GetAvailableHotelRoomTypesQueryValidator()
    {
        RuleFor(x => x.HotelId)
            .GreaterThan(0)
            .WithMessage("Hotel ID must be greater than zero.")
            .WithErrorCode("validation.roomtype.hotelid.invalid");

        RuleFor(x => x.CheckIn)
            .Must(date => date >= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Booking.CheckInInPast")
            .WithErrorCode("validation.roomtype.checkin.invalid");

        RuleFor(x => x.CheckOut)
            .Must((query, checkOut) => checkOut > query.CheckIn)
            .WithMessage("Check-out date must be after check-in date.")
            .WithErrorCode("Booking.InvalidCheckOut");

        RuleFor(x => x.Guests)
            .GreaterThan(0)
            .WithMessage("Number of guests must be greater than zero.")
            .WithErrorCode("validation.roomtype.guests.invalid");

        RuleFor(x => x.PageNumer)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than zero.")
            .WithErrorCode("Validation.Common.InvalidPageNumber");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(10, 30)
            .WithMessage("Page size must be between 1 and 30.")
            .WithErrorCode("validation.pagination.availablepagesize.invalid");
    }
}