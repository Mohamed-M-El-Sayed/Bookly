namespace Bookly.Application.Features.HotelImages.Commands.AddHotelImage;

public record AddHotelImageResponse(int HotelId, string Url, bool IsPrimary)
{
}
