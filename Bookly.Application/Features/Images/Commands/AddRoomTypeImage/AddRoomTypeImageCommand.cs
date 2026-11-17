using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Images.Commands.AddRoomTypeImage;

public class AddRoomTypeImageCommand : IRequest<Result<AddRoomTypeImageResponse>>
{
    public int RoomTypeId { get; set; }
    public string Url { get; set; } = null!;
    public bool IsPrimary { get; set; }
}
