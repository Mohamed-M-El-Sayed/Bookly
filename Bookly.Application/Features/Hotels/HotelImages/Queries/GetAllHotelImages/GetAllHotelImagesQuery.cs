using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Hotels.HotelImages.Queries.GetAllHotelImages;

public class GetAllHotelImagesQuery(int hotelId) : IRequest<Result<List<GetAllHotelImagesResponse>>>
{
    public int HotelId { get; } = hotelId;
}
