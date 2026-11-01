using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Bookings.Queries.GetBookingById
{
    public record GetBookingByIdQuery(int id) : IRequest<Result<BookingDetailsResponse>>;
}