using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Bookings.Commands.ReleaseBookingAvailability;

public record ReleaseBookingAvailabilityCommand(int BookingId) : IRequest<Result>;

