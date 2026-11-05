using Bookly.Application.Common.Results;
using Bookly.Application.Features.Payments.Commands.CreateCheckoutSession;
using Bookly.Application.Features.Payments.Commands.HandlePaymentWebhook;

namespace Bookly.Application.Contracts.Services.Payments;

public interface IPaymentService
{
    Task<Result<CheckoutSessionResult>> CreateCheckoutSessionAsync(int bookingId, decimal amount, string currency,
      CancellationToken cancellationToken = default);
    Result<PaymentWebhookResult> ParseWebhookEvent(string payload, string signature);
}

