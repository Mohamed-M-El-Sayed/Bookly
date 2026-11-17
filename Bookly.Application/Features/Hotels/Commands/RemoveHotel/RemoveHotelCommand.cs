using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Services.Caching;
using MediatR;

namespace Bookly.Application.Features.Hotels.Commands.RemoveHotel;

public record RemoveHotelCommand(int HotelId) : IRequest<Result>, ICacheInvalidationRequest
{
    public IReadOnlyCollection<string> Tags => [CacheTags.HotelSearch];
}