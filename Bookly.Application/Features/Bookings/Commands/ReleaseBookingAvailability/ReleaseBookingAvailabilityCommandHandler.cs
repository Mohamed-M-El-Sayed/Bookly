using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.Bookings.Messages;
using Bookly.Application.Features.Bookings.Specifications;
using Bookly.Domain.Entities.Bookings;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Bookings.Commands.ReleaseBookingAvailability;

public class ReleaseBookingAvailabilityCommandHandler(IUnitOfWork unitOfWork,
    ILogger<ReleaseBookingAvailabilityCommandHandler> logger) : IRequestHandler<ReleaseBookingAvailabilityCommand, Result>
{
    public async Task<Result> Handle(ReleaseBookingAvailabilityCommand request, CancellationToken cancellationToken)
    {
        var booking = await unitOfWork.Repository<Booking>().GetEntityWithSpecAsync(
                new BookingForAvailabilityReleaseSpecification(request.BookingId));

        if (booking is null)
            return Result.Failure(BookingErrors.NotFound());

        foreach (var bookingRoom in booking.BookingRooms)
        {
            var released = bookingRoom.RoomType.TryRelease(
                booking.CheckIn,
                booking.CheckOut);

            if (!released)
            {
                logger.LogWarning("Failed to release availability for Booking {BookingId}.",
                    booking.Id);

                return Result.Failure(
                    BookingErrors.AvailabilityReleaseFailed());
            }
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Availability released successfully for Booking {BookingId}.",
            booking.Id);
        return Result.Success();
    }
}