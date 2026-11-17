using Bookly.Application.Common.Results;
using MediatR;
using System.Text.Json.Serialization;

namespace Bookly.Application.Features.SeasonalPricing.Commands.UpdateSeasonalPrice;

public class UpdateSeasonalPriceCommand : IRequest<Result>
{
    [JsonIgnore]
    public int Id { get; set; }
    [JsonIgnore]
    public int RoomTypeId { get; set; }
    public string Name { get; set; } = null!;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public decimal PricePerNight { get; set; }
}



