using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Bookings.Commands.CreateBooking;

public record CreateBookingCommand(
    int HotelId,
    int RoomTypeId,
    int? BoardOptionId,
    DateOnly CheckIn,
    DateOnly CheckOut,
    int Guests,
    string GuestName,
    string PhoneNumber) : IRequest<Result<CreateBookingResponse>>;