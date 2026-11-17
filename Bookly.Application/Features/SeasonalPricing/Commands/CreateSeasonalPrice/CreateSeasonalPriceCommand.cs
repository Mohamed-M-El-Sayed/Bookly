using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Services.Caching;
using MediatR;
using System.Text.Json.Serialization;

namespace Bookly.Application.Features.SeasonalPricing.Commands.CreateSeasonalPrice;

public class CreateSeasonalPriceCommand : IRequest<Result<CreateSeasonalPriceResponse>>, ICacheInvalidationRequest
{
    [JsonIgnore]
    public int RoomTypeId { get; set; }

    public string Name { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public decimal PricePerNight { get; set; }
    public IReadOnlyCollection<string> Tags => [CacheTags.HotelSearch];

}
