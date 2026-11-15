using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.SeasonalPricing.Commands.DeleteSeasonalPrice;

public record DeleteSeasonalPriceCommand(int Id) : IRequest<Result>;
