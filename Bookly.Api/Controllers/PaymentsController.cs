using Bookly.Application.Features.Payments.Commands.CreateCheckoutSession;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController(IMediator mediator) : ApiControllerBase
    {
        /// <summary>
        /// Creates a checkout session for a booking.
        /// </summary>
        [HttpPost("checkout")]
        public async Task<IActionResult> CreateCheckoutSession([FromBody] CreateCheckoutSessionCommand command)
        {
            var result = await mediator.Send(command);
            return Respond(result);
        }

    }
}
