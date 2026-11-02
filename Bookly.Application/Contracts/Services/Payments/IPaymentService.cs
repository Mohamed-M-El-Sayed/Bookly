using Bookly.Application.Common.Results;

namespace Bookly.Application.Contracts.Services.Payments;

public interface IPaymentService
{
    Task<Result<CheckoutSessionResult>> CreateCheckoutSessionAsync(int bookingId, decimal amount, string currency,
      CancellationToken cancellationToken = default);

}
