using Bookly.Application.Common.Results;
using Bookly.Application.Features.BoardOptions.Queries.GetRoomTypeBoardOptions;
using MediatR;

namespace Bookly.Application.Features.BoardOptions.Queries.GetBoardOptionById;

public record GetBoardOptionByIdQuery(int Id, int RoomTypeId) : IRequest<Result<BoardOptionResponse>>;
