using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.BackgroundJobs;
using Bookly.Application.Contracts.Services.Users;
using Bookly.Application.Features.Auth.Messages;
using Bookly.Application.Features.Bookings.Messages;
using Bookly.Application.Features.Bookings.Specifications;
using Bookly.Application.Features.RoomTypes.Errors;
using Bookly.Domain.Entities.Bookings;
using Bookly.Domain.Entities.RoomTypes;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Bookings.Commands.CreateBooking;

public class CreateBookingCommandHandler(IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    IBookingExpiryScheduler bookingExpiryScheduler,
    ILogger<CreateBookingCommandHandler> logger) : IRequestHandler<CreateBookingCommand, Result<CreateBookingResponse>>
{
    public async Task<Result<CreateBookingResponse>> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (userId is null)
            return Result<CreateBookingResponse>.Failure(AuthErrors.Unauthorized());
        logger.LogInformation("Creating booking for user {UserId}", userId);
        var roomType = await unitOfWork.Repository<RoomType>().GetEntityWithSpecAsync(
            new RoomTypeForBookingSpecification(request.RoomTypeId, request.CheckIn, request.CheckOut, request.BoardOptionId), cancellationToken);
        if (roomType is null)
            return Result<CreateBookingResponse>.Failure(RoomTypeErrors.NotFound());
        if (request.BoardOptionId.HasValue && !roomType.BoardOptions.Any(b => b.Id == request.BoardOptionId.Value))
            return Result<CreateBookingResponse>.Failure(BookingErrors.InvalidBoardOption());
        if (request.Guests > roomType.MaxOccupancy)
            return Result<CreateBookingResponse>.Failure(BookingErrors.ExceedsMaxOccupancy());
        if (!roomType.TryReserve(request.CheckIn, request.CheckOut))
            return Result<CreateBookingResponse>.Failure(BookingErrors.RoomNotAvailable());
        var nights =
            request.CheckOut.DayNumber - request.CheckIn.DayNumber;
        var price = roomType.TotalPriceForStay(request.CheckIn, request.CheckOut, request.BoardOptionId);
        var booking = Booking.Create(
            userId.Value,
            roomType.HotelId,
            request.CheckIn,
            request.CheckOut,
            request.GuestName,
            request.PhoneNumber);
        booking.AddRoom(request.RoomTypeId, null, request.Guests, price);
        booking.AddCancellationRules(roomType.Hotel.CancellationPolicy.Rules.Select(r =>
                (r.DaysBeforeCheckIn, r.RefundPercentage)));

        await unitOfWork.Repository<Booking>().AddAsync(booking, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        await bookingExpiryScheduler.ScheduleAsync(booking.Id, TimeSpan.FromMinutes(15), cancellationToken);
        return Result<CreateBookingResponse>.Success(new CreateBookingResponse(
          booking.Reference,
          booking.Status,
          booking.CheckIn,
          booking.CheckOut,
          nights,
          price,
          booking.ExpiresAt));
    }
}