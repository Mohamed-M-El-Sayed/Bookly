using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.BoardOptions.Queries.GetRoomTypeBoardOptions;

public record GetRoomTypeBoardOptionsQuery(int RoomTypeId) : IRequest<Result<List<BoardOptionResponse>>>;
