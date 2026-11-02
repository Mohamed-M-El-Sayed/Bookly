using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Payments;
using Bookly.Application.Contracts.Services.Users;
using Bookly.Application.Features.Auth.Messages;
using Bookly.Application.Features.Bookings.Messages;
using Bookly.Application.Features.Bookings.Specifications;
using Bookly.Domain.Entities.Bookings;
using Bookly.Domain.Entities.Payments;
using Bookly.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Payments.Commands.CreateCheckoutSession;

public class CreateCheckoutSessionCommandHandler(IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    IPaymentService paymentService,
    ILogger<CreateCheckoutSessionCommandHandler> logger) : IRequestHandler<CreateCheckoutSessionCommand, Result<CheckoutSessionResult>>
{
    public async Task<Result<CheckoutSessionResult>> Handle(CreateCheckoutSessionCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
           "Starting checkout session creation for Booking {BookingId}.",
           request.BookingId);
        var userId = currentUserService.UserId;
        if (userId is null)
            return Result<CheckoutSessionResult>.Failure((AuthErrors.Unauthorized()));
        var booking = await unitOfWork.Repository<Booking>().GetEntityWithSpecAsync(new BookingWithBookingRoomSpecification(request.BookingId));
        if (booking is null)
            return Result<CheckoutSessionResult>.Failure(BookingErrors.NotFound());
        if (booking.UserId != userId)
            return Result<CheckoutSessionResult>.Failure(BookingErrors.AccessDenied());
        if (booking.Status != BookingStatus.Pending)
            return Result<CheckoutSessionResult>.Failure(
                BookingErrors.InvalidStatus());
        if (booking.ExpiresAt <= DateTime.UtcNow)
            return Result<CheckoutSessionResult>.Failure(
                BookingErrors.Expired());
        var payment = new Payment(request.BookingId, booking.TotalPrice);
        await unitOfWork.Repository<Payment>().AddAsync(payment);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation(
            "Payment {PaymentId} created for Booking {BookingId}.",
            payment.Id,
            booking.Id);
        var checkoutResult = await paymentService.CreateCheckoutSessionAsync(
            booking.Id,
            payment.Amount,
            "egp",
            cancellationToken);
        if (!checkoutResult.IsSuccess)
        {
            logger.LogError(
                "Failed to create checkout session for Payment {PaymentId}.",
                payment.Id);
            return Result<CheckoutSessionResult>.Failure(checkoutResult.Error!);
        }
        payment.SetPaymentSessionId(checkoutResult.Value.SessionId);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation(
            "Checkout session created successfully for Booking {BookingId}.",
            booking.Id);
        return Result<CheckoutSessionResult>.Success(checkoutResult.Value);
    }
}
