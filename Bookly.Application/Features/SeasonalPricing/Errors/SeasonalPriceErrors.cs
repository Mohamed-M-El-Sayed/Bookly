using Bookly.Application.Common.Results;

namespace Bookly.Application.Features.SeasonalPricing.Errors;

public static class SeasonalPriceErrors
{
    public static Error NotFound() =>
         ErrorFactory.NotFound("SeasonalPrice.NotFound", $"Seasonal price was not found.");
}
