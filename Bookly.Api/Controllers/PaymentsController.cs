using Bookly.Application.Features.Payments.Commands.CreateCheckoutSession;
using Bookly.Application.Features.Payments.Commands.HandlePaymentWebhook;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

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
        /// <summary>
        /// Handles incoming payment webhooks from Stripe.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("webhook")]
        [DisableRateLimiting]
        public async Task<IActionResult> HandleWebhook(CancellationToken cancellationToken)
        {
            using var reader = new StreamReader(Request.Body);
            var payload = await reader.ReadToEndAsync(cancellationToken);

            var signature = Request.Headers["Stripe-Signature"].ToString();
            if (string.IsNullOrWhiteSpace(signature))
                return BadRequest();
            var result = await mediator.Send(
                new HandlePaymentWebhookCommand(payload, signature),
                cancellationToken);
            return Respond(result);
        }
    }
}
