using Bookly.Application.Common.Results;

namespace Bookly.Application.Contracts.Services.Hotels;

public interface IHotelAccessService
{
    Task<Result> EnsureCurrentUserManagesAsync(int hotelId, CancellationToken cancellationToken = default);

}
