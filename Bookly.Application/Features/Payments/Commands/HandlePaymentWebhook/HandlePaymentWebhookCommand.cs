using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Payments.Commands.HandlePaymentWebhook;

public record HandlePaymentWebhookCommand(string Payload, string Signature) : IRequest<Result>;
