using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.HotelImages.Queries.GetAllHotelImages;

public class GetAllHotelImagesQuery(int hotelId) : IRequest<Result<List<HotelImagesResponse>>>
{
    public int HotelId { get; } = hotelId;
}
