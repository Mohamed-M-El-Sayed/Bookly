using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.SeasonalPricing.Queries.GetSeasonalPriceById;

public record GetSeasonalPriceByIdQuery(int Id) : IRequest<Result<SeasonalPriceResponse>>;
