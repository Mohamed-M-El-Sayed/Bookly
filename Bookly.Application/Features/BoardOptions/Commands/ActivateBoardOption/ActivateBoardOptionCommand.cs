using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.BoardOptions.Commands.ActivateBoardOption;

public record ActivateBoardOptionCommand(int Id, int RoomTypeId) : IRequest<Result>;
