namespace Bookly.Infrastructure.Services.Payments;

public static class StripeEvents
{
    public const string PaymentSucceeded = "checkout.session.completed";
    public const string PaymentAsyncSucceeded = "checkout.session.async_payment_succeeded";
    public const string PaymentFailed = "checkout.session.async_payment_failed";
    public const string PaymentRefunded = "charge.refunded";
}
