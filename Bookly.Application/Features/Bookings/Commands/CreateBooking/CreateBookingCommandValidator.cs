
using FluentValidation;

namespace Bookly.Application.Features.Bookings.Commands.CreateBooking;

public sealed class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingCommandValidator()
    {
        RuleFor(x => x.HotelId)
            .GreaterThan(0)
            .WithErrorCode("Booking.InvalidHotelId")
            .WithMessage("A valid hotel ID is required.");

        RuleFor(x => x.RoomTypeId)
            .GreaterThan(0)
            .WithErrorCode("Booking.InvalidRoomTypeId")
            .WithMessage("A valid room type ID is required.");

        RuleFor(x => x.BoardOptionId)
            .GreaterThan(0)
            .When(x => x.BoardOptionId.HasValue)
            .WithErrorCode("Booking.InvalidBoardOptionId")
            .WithMessage("A valid board option ID is required.");

        RuleFor(x => x.CheckIn)
            .Must(checkIn =>
                checkIn >= DateOnly.FromDateTime(DateTime.UtcNow))
            .WithErrorCode("Booking.CheckInInPast")
            .WithMessage("Check-in date cannot be in the past.");

        RuleFor(x => x.CheckOut)
            .GreaterThan(x => x.CheckIn)
            .WithErrorCode("Booking.InvalidCheckOut")
            .WithMessage("Check-out date must be after check-in date.");

        RuleFor(x => x.Guests)
            .GreaterThan(0)
            .WithErrorCode("Booking.InvalidGuests")
            .WithMessage("At least one guest is required.");

        RuleFor(x => x.GuestName)
            .NotEmpty()
            .WithErrorCode("Booking.GuestNameRequired")
            .WithMessage("Guest name is required.")
            .MaximumLength(100)
            .WithErrorCode("Booking.GuestNameTooLong")
            .WithMessage("Guest name cannot exceed 100 characters.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithErrorCode("Booking.PhoneNumberRequired")
            .WithMessage("Phone number is required.")
            .Matches(@"^\+201[0125][0-9]{8}$")
            .WithErrorCode("Booking.InvalidEgyptianPhoneNumber")
            .WithMessage("Please enter a valid Egyptian phone number.");
    }
}
