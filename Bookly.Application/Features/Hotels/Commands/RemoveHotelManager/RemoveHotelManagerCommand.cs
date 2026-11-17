using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Hotels.Commands.RemoveHotelManager;

public record RemoveHotelManagerCommand(int HotelId,
    Guid UserId) : IRequest<Result>;
