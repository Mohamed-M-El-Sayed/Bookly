namespace Bookly.Application.Features.Hotels.HotelImages.Queries.GetAllHotelImages;

public record GetAllHotelImagesResponse(int Id, int HotelId, string Url, bool IsPrimary);
