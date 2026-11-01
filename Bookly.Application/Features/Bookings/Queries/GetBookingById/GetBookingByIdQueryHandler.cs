using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Users;
using Bookly.Application.Features.Auth.Messages;
using Bookly.Application.Features.Bookings.Messages;
using Bookly.Application.Features.Bookings.Specifications;
using Bookly.Domain.Entities.Bookings;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Bookings.Queries.GetBookingById;

public class GetBookingByIdQueryHandler(IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    ILogger<GetBookingByIdQueryHandler> logger) : IRequestHandler<GetBookingByIdQuery, Result<BookingDetailsResponse>>
{
    public async Task<Result<BookingDetailsResponse>> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving booking with ID {BookingId}.", request.id);
        var userId = currentUserService.UserId;
        if (userId is null)
            return Result<BookingDetailsResponse>.Failure(AuthErrors.Unauthorized());
        var booking = await unitOfWork.Repository<Booking>().GetEntityWithSpecAsync(new BookingByIdSpecification(request.id, userId.Value));
        if (booking is null)
            return Result<BookingDetailsResponse>.Failure(BookingErrors.NotFound());
        var response = booking.Adapt<BookingDetailsResponse>();
        logger.LogInformation("Booking {BookingId} retrieved successfully by user {UserId}.",
            request.id, userId.Value);
        return Result<BookingDetailsResponse>.Success(response);
    }
}
