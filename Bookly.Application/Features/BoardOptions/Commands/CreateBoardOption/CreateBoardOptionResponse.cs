using Bookly.Domain.Enums;

namespace Bookly.Application.Features.BoardOptions.Commands.CreateBoardOption;

public record class CreateBoardOptionResponse(int RoomTypeId, BoardType Type);


