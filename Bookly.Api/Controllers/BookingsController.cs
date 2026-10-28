using Bookly.Application.Features.Bookings.Commands.CreateBooking;
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
        [HttpGet("{hotelId:int}")]
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

    }
}