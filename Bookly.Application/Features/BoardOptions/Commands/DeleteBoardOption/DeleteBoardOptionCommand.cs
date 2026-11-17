using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.BoardOptions.Commands.DeleteBoardOption;

public record DeleteBoardOptionCommand(int Id, int RoomTypeId) : IRequest<Result>;
