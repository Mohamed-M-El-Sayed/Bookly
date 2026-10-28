using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Bookings.Commands.ExpireBooking;

public record ExpireBookingCommand(int BookingId) : IRequest<Result>;
