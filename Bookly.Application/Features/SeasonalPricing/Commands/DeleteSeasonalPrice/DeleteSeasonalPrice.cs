using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Services.Caching;
using MediatR;

namespace Bookly.Application.Features.SeasonalPricing.Commands.DeleteSeasonalPrice;

public record DeleteSeasonalPriceCommand(int Id) : IRequest<Result>, ICacheInvalidationRequest
{
    public IReadOnlyCollection<string> Tags => [CacheTags.HotelSearch];
}