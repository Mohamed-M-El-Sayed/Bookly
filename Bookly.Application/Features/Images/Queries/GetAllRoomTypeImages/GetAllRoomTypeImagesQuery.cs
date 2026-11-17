using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Images.Queries.GetAllRoomTypeImages;

public class GetAllRoomTypeImagesQuery(int roomTypeId) : IRequest<Result<List<RoomTypeImagesResponse>>>
{
    public int RoomTypeId { get; } = roomTypeId;
}
