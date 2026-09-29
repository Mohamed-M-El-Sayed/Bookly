namespace Bookly.Application.Features.Hotels.HotelImages.Commands.AddHotelImage;

public record AddHotelImageResponse(int HotelId, string Url, bool IsPrimary)
{
}
