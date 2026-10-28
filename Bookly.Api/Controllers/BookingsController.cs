using Bookly.Application.Features.Bookings.Commands.CreateBooking;
using Bookly.Application.Features.Bookings.Queries.GetMyBookings;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController(IMediator mediator) : ApiControllerBase
    {
        /// <summary>
        /// Retrieves the current user's bookings.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetMyBooking([FromQuery] GetMyBookingsQuery query, CancellationToken cancellationToken)
        {
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
