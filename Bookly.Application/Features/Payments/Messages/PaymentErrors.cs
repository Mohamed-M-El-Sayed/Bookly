using Bookly.Application.Common.Results;

namespace Bookly.Application.Features.Payments.Messages;

public static class PaymentErrors
{
    public static Error NotFound() =>
        ErrorFactory.NotFound("Payment.NotFound", "Payment not found");
    public static Error CheckoutSessionCreationFailed() =>
        ErrorFactory.Internal("Payment.CheckoutSessionCreationFailed",
            "Failed to create checkout session.");
    public static Error UnsupportedWebhookEvent() =>
        ErrorFactory.Validation("Payment.UnsupportedWebhookEvent",
       "The Stripe webhook event is not supported.");
    public static Error InvalidWebhookSignature() =>
        ErrorFactory.Unauthorized("Payment.InvalidWebhookSignature",
        "The Stripe webhook signature is invalid.");
    public static Error AmountMisMatch() =>
        ErrorFactory.Validation("Payment.AmountMisMatch",
        "The payment amount does not match the expected amount.");
}
