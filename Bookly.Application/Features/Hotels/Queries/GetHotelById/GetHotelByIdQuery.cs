using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Hotels.Queries.GetHotelById;

public record GetHotelByIdQuery(int HotelId) : IRequest<Result<HotelResponse>>;
