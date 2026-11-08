using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.Bookings.Specifications;
using Bookly.Domain.Entities.Bookings;
using Bookly.Domain.Entities.RoomTypes;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Bookings.Commands.ExpireBooking;

public class ExpireBookingCommandHandler(IUnitOfWork unitOfWork,
    ILogger<ExpireBookingCommandHandler> logger) : IRequestHandler<ExpireBookingCommand, Result>
{
    public async Task<Result> Handle(ExpireBookingCommand request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var booking = await unitOfWork.Repository<Booking>().GetEntityWithSpecAsync(
            new BookingForExpirySpecification(request.BookingId), cancellationToken);
        if (booking is null || !booking.TryExpire(now))
        {
            logger.LogInformation("Booking {BookingId} was not expired because it is missing, not pending, or not due yet",
                request.BookingId);
            return Result.Success();
        }
        var bookingRoom = booking.BookingRooms.SingleOrDefault();
        if (bookingRoom is null)
        {
            logger.LogError("Booking {BookingId} has no associated room",
                booking.Id);
            return Result.Success();
        }
        var roomType = await unitOfWork.Repository<RoomType>()
            .GetEntityWithSpecAsync(
                new RoomTypeForExpirySpecification(
                    bookingRoom.RoomTypeId,
                    booking.CheckIn,
                    booking.CheckOut), cancellationToken);
        if (roomType is null)
        {
            logger.LogError(
                "Room type {RoomTypeId} for booking {BookingId} was not found",
                bookingRoom.RoomTypeId,
                booking.Id);

            return Result.Success();
        }
        if (!roomType.TryRelease(booking.CheckIn, booking.CheckOut))
        {
            logger.LogError(
                "Could not release availability for booking {BookingId}, room type {RoomTypeId}",
                booking.Id,
                bookingRoom.RoomTypeId);
        }
        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Booking {Reference} expired and its room availability was released",
            booking.Reference);
        return Result.Success();
    }
}
