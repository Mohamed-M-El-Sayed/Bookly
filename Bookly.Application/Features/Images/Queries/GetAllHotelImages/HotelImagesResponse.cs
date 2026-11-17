namespace Bookly.Application.Features.HotelImages.Queries.GetAllHotelImages;

public record HotelImagesResponse(int Id, int HotelId, string Url, bool IsPrimary);
