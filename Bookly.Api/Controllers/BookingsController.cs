using Bookly.Application.Features.Bookings.Commands.CreateBooking;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController(IMediator mediator) : ApiControllerBase
    {
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
