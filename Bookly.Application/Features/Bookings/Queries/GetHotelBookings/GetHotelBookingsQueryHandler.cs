using Bookly.Application.Common.Models;
using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Hotels;
using Bookly.Application.Features.Bookings.Shared;
using Bookly.Application.Features.Bookings.Specifications;
using Bookly.Domain.Entities.Bookings;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Bookings.Queries.GetHotelBookings;

public class GetHotelBookingsQueryHandler(IUnitOfWork unitOfWork,
    IHotelAccessService hotelAccessService,
    ILogger<GetHotelBookingsQueryHandler> logger) : IRequestHandler<GetHotelBookingsQuery, Result<PagedResult<BookingResponse>>>
{
    public async Task<Result<PagedResult<BookingResponse>>> Handle(GetHotelBookingsQuery request, CancellationToken cancellationToken)
    {
        var result = await hotelAccessService.EnsureCurrentUserManagesAsync(request.HotelId);
        if (result.IsFailure)
            return Result<PagedResult<BookingResponse>>.Failure(result.Error!);
        var specification = new HotelBookingsSpecification(request.HotelId,
            request.PageIndex,
            request.PageSize,
            request.Status,
            request.CheckInFrom,
            request.CheckOutTo);
        var booking = await unitOfWork.Repository<Booking>().GetAllWithSpecAsync(
            specification, cancellationToken);
        var response = booking.Adapt<IReadOnlyList<BookingResponse>>();
        var count = await unitOfWork.Repository<Booking>().CountAsync(specification);
        return Result<PagedResult<BookingResponse>>.Success(
            new(response, count, request.PageIndex, request.PageSize));
    }
}
