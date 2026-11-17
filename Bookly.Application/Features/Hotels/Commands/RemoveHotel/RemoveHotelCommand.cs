using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Hotels.Commands.RemoveHotel;

public record RemoveHotelCommand(int HotelId) : IRequest<Result>;
