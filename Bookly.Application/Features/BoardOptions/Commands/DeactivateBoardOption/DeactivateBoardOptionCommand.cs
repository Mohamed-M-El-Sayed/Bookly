using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.BoardOptions.Commands.DeactivateBoardOption;

public record DeactivateBoardOptionCommand(int Id, int RoomTypeId) : IRequest<Result>;
