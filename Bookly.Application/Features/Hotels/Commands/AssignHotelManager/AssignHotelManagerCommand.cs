using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Hotels.Commands.AssignHotelManager;

public record AssignHotelManagerCommand(int HotelId,
 Guid UserId) : IRequest<Result>;
