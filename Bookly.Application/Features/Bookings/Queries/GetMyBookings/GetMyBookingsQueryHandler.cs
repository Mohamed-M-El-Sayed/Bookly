using Bookly.Application.Common.Models;
using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Users;
using Bookly.Application.Features.Auth.Messages;
using Bookly.Application.Features.Bookings.Shared;
using Bookly.Application.Features.Bookings.Specifications;
using Bookly.Domain.Entities.Bookings;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Bookings.Queries.GetMyBookings;

public class GetMyBookingsQueryHandler(IUnitOfWork unitOfWork,
    ILogger<GetMyBookingsQueryHandler> logger,
    ICurrentUserService currentUserService) : IRequestHandler<GetMyBookingsQuery, Result<PagedResult<BookingResponse>>>
{
    public async Task<Result<PagedResult<BookingResponse>>> Handle(GetMyBookingsQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        logger.LogInformation("Getting Booking for User {UserId}", userId);
        if (userId is null)
            return Result<PagedResult<BookingResponse>>.Failure(AuthErrors.Unauthorized());
        var specification = new MyBookingsSpecification(
            userId.Value, request.PageIndex, request.PageSize, request.Status);
        var bookings = await unitOfWork.Repository<Booking>().GetAllWithSpecAsync(
            specification, cancellationToken);
        var count = await unitOfWork.Repository<Booking>().CountAsync(specification, cancellationToken);
        var response = bookings.Adapt<IReadOnlyList<BookingResponse>>();
        return Result<PagedResult<BookingResponse>>.Success(
            new PagedResult<BookingResponse>(response, count, request.PageIndex, request.PageSize));
    }
}
