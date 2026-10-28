using Bookly.Application.Common.Models;
using Bookly.Application.Common.Results;
using Bookly.Application.Features.Bookings.Shared;
using Bookly.Domain.Enums;
using MediatR;

namespace Bookly.Application.Features.Bookings.Queries.GetMyBookings;

public record GetMyBookingsQuery(int PageIndex,
    int PageSize, BookingStatus? Status) : IRequest<Result<PagedResult<BookingResponse>>>;

