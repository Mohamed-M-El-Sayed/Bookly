namespace Bookly.Application.Features.Hotels.Queries
{
    public record SearchHotelsResponse(
    int Id,
    string Name,
    int? StarRating,
    string City,
    string Country,
    decimal MinPricePerNight,
    string? PrimaryImageUrl);
}
