using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Services.Payments;
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
        var stripeClient = new StripeClient("REMOVED_STRIPE_SECRET");
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
}
