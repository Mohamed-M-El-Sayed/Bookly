namespace Bookly.Application.Features.Payments.Commands.HandlePaymentWebhook;

public record PaymentWebhookResult(
    string EventId,
    PaymentEventType EventType,
    string SessionId,
    string? TransactionId,
    decimal Amount,
    string Currency,
    string PaymentStatus);

public enum PaymentEventType
{
    PaymentSucceeded,
    PaymentFailed,
    PaymentRefunded
}