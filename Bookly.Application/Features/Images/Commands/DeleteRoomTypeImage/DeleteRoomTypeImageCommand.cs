using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Images.Commands.DeleteRoomTypeImage;

public record DeleteRoomTypeImageCommand(int RoomTypeId, int ImageId)
    : IRequest<Result>;
