using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Services.Payments;
using Bookly.Application.Features.Payments.Commands.CreateCheckoutSession;
using Bookly.Application.Features.Payments.Commands.HandlePaymentWebhook;
using Bookly.Application.Features.Payments.Messages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace Bookly.Infrastructure.Services.Payments;

public class StripePaymentService : IPaymentService
{
    private readonly StripeOptions _options;
    private readonly ILogger<StripePaymentService> _logger;
    private readonly SessionService _sessionService;
    public StripePaymentService(IOptions<StripeOptions> options, ILogger<StripePaymentService> logger)
    {
        _options = options.Value;
        _logger = logger;
        var stripeClient = new StripeClient(_options.SecretKey);
        _sessionService = new SessionService(stripeClient);
    }
    public async Task<Result<CheckoutSessionResult>> CreateCheckoutSessionAsync(int bookingId, decimal amount, string currency,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
           "Creating Stripe checkout session for Booking {BookingId}.",
           bookingId);
        try
        {
            var options = new SessionCreateOptions
            {
                Mode = "payment",
                SuccessUrl = _options.SuccessUrl,
                CancelUrl = _options.CancelUrl,
                ClientReferenceId = bookingId.ToString(),
                LineItems =
                [
                     new SessionLineItemOptions
                        {
                            Quantity = 1,
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                Currency = currency,
                                UnitAmount = (long)(amount * 100),
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = $"Booking #{bookingId}"
                                }
                            }
                        }
                ]

            };
            Session session = await _sessionService.CreateAsync(
                options,
                cancellationToken: cancellationToken);
            _logger.LogInformation(
                "Stripe checkout session {SessionId} created successfully for Booking {BookingId}.",
                session.Id,
                bookingId);
            return Result<CheckoutSessionResult>.Success(new CheckoutSessionResult(
               session.Id,
               session.Url));
        }
        catch (StripeException ex)
        {
            _logger.LogError(
                ex,
                "Stripe error while creating checkout session for Booking {BookingId}.",
                bookingId);
            return Result<CheckoutSessionResult>.Failure(PaymentErrors.CheckoutSessionCreationFailed());
        }


    }

    public Result<PaymentWebhookResult> ParseWebhookEvent(string payload, string signature)
    {
        try
        {
            var stripeEvent = EventUtility.ConstructEvent(
                payload,
                signature,
                _options.WebhookSecret);
            switch (stripeEvent.Data.Object)
            {
                case Session session:
                    {
                        PaymentEventType? eventType = stripeEvent.Type switch
                        {
                            StripeEvents.PaymentSucceeded
                                when session.PaymentStatus == "paid"
                                => PaymentEventType.PaymentSucceeded,

                            StripeEvents.PaymentAsyncSucceeded
                                when session.PaymentStatus == "paid"
                                => PaymentEventType.PaymentSucceeded,

                            StripeEvents.PaymentFailed
                                => PaymentEventType.PaymentFailed,

                            _ => null
                        };

                        if (eventType is null)
                        {
                            _logger.LogWarning(
                                "Unsupported Stripe event {EventType}.",
                                stripeEvent.Type);

                            return Result<PaymentWebhookResult>.Failure(
                                PaymentErrors.UnsupportedWebhookEvent());
                        }

                        var result = new PaymentWebhookResult(
                            stripeEvent.Id,
                            eventType.Value,
                            session.Id,
                            session.PaymentIntentId,
                            (session.AmountTotal ?? 0) / 100m,
                            session.Currency,
                            session.PaymentStatus);

                        return Result<PaymentWebhookResult>.Success(result);
                    }

                case Charge charge
                    when stripeEvent.Type == StripeEvents.PaymentRefunded:
                    {
                        var result = new PaymentWebhookResult(
                            stripeEvent.Id,
                            PaymentEventType.PaymentRefunded,
                            null!,
                            charge.PaymentIntentId,
                            charge.AmountRefunded / 100m,
                            charge.Currency,
                            charge.Refunded ? "refunded" : "partially_refunded");

                        return Result<PaymentWebhookResult>.Success(result);
                    }

                default:
                    {
                        _logger.LogWarning(
                            "Unexpected Stripe webhook object for event {EventId}, type {EventType}.",
                            stripeEvent.Id,
                            stripeEvent.Type);

                        return Result<PaymentWebhookResult>.Failure(
                            PaymentErrors.UnsupportedWebhookEvent());
                    }
            }
        }
        catch (StripeException ex)
        {
            _logger.LogError(
                ex,
                "Stripe error while parsing webhook event.");

            return Result<PaymentWebhookResult>.Failure(
                PaymentErrors.InvalidWebhookSignature());
        }
    }
}
