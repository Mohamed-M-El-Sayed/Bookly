namespace Bookly.Application.Features.SeasonalPricing.Queries.GetSeasonalPriceById;

public record SeasonalPriceResponse(
    int Id,
    int RoomTypeId,
    string Name,
    DateOnly StartDate,
    DateOnly EndDate,
    decimal PricePerNight);
