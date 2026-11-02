using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Services.Payments;
using MediatR;

namespace Bookly.Application.Features.Payments.Commands.CreateCheckoutSession;

public record CreateCheckoutSessionCommand(int BookingId) : IRequest<Result<CheckoutSessionResult>>;