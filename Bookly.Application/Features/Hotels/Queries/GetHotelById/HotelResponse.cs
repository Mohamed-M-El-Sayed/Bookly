namespace Bookly.Application.Features.Hotels.Queries.GetHotelById;

public record HotelResponse(
    int Id,
    string Name,
    string Description,
    string DescriptionAr,
    int? StarRating,
    string Street,
    string City,
    string State,
    string Country,
    string Phone,
    string Email,
    string? Website);
