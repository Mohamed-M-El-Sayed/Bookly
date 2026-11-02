using Bookly.Application.Common.Results;

namespace Bookly.Application.Features.Payments.Messages;

public static class PaymentErrors
{
    public static Error CheckoutSessionCreationFailed() =>
        ErrorFactory.Internal("Payment.CheckoutSessionCreationFailed",
            "Failed to create checkout session.");
}
