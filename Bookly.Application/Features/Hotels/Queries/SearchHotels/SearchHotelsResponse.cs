namespace Bookly.Application.Features.Hotels.Queries.SearchHotels;

public class SearchHotelsResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int? StarRating { get; set; }
    public string City { get; set; } = null!;
    public string Country { get; set; } = null!;
    public decimal MinPricePerNight { get; set; }
    public string? PrimaryImageUrl { get; set; }
}
