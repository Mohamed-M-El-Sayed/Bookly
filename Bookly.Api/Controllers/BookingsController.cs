using Bookly.Application.Features.Bookings.Commands.CancelBooking;
using Bookly.Application.Features.Bookings.Commands.CreateBooking;
using Bookly.Application.Features.Bookings.Queries.GetBookingById;
using Bookly.Application.Features.Bookings.Queries.GetHotelBookings;
using Bookly.Application.Features.Bookings.Queries.GetMyBookings;
using Bookly.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController(IMediator mediator) : ApiControllerBase
    {
        /// <summary>
        /// Retrieves the details of a specific booking.
        /// </summary>
        [HttpGet("{bookingId:int}")]
        public async Task<IActionResult> GetBookingById(
            int bookingId,
            CancellationToken cancellationToken)
        {
            var query = new GetBookingByIdQuery(bookingId);
            var result = await mediator.Send(query, cancellationToken);
            return Respond(result);
        }

        /// <summary>
        /// Retrieves the current user's bookings.
        /// </summary>
        [HttpGet]
        [Authorize(Roles = Roles.Customer)]
        public async Task<IActionResult> GetMyBooking([FromQuery] GetMyBookingsQuery query, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(query, cancellationToken);
            return Respond(result);
        }
        /// <summary>
        /// Retrieves booking for specific hotel (Hotel manger only).
        /// </summary>
        [HttpGet("hotel/{hotelId:int}")]
        [Authorize(Roles = Roles.Manager)]
        public async Task<IActionResult> GetHotelBookings([FromRoute] int hotelId, [FromQuery] GetHotelBookingsQuery query, CancellationToken cancellationToken)
        {
            query.HotelId = hotelId;
            var result = await mediator.Send(query, cancellationToken);
            return Respond(result);
        }
        /// <summary>
        /// Creates a new booking.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookingCommand command, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command, cancellationToken);
            return Respond(result);
        }
        /// <summary>
        /// Requests cancellation of a booking.
        /// </summary>
        [HttpPost("{bookingId:int}/cancel")]
        public async Task<IActionResult> CancelBooking(int bookingId, [FromBody] CancelBookingCommand command,
            CancellationToken cancellationToken)
        {
            command.BookingId = bookingId;
            var result = await mediator.Send(command, cancellationToken);
            return Respond(result, StatusCodes.Status202Accepted);
        }

    }
}