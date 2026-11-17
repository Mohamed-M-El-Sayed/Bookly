using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Services.Caching;
using MediatR;

namespace Bookly.Application.Features.Bookings.Commands.CreateBooking;

public record CreateBookingCommand(int HotelId,
    int RoomTypeId,
    int? BoardOptionId,
    DateOnly CheckIn,
    DateOnly CheckOut,
    int Guests,
    string GuestName,
    string Email,
    string PhoneNumber) : IRequest<Result<CreateBookingResponse>>, ICacheInvalidationRequest
{
    public IReadOnlyCollection<string> Tags => [CacheTags.HotelSearch];
}