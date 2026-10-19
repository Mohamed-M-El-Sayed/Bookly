using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Services.Hotels;
using Bookly.Application.Contracts.Services.Users;
using Bookly.Application.Features.Auth.Messages;
using Bookly.Application.Features.Hotels.Errors;
using Bookly.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookly.Infrastructure.Services.Hotels;

public class HotelAccessService(AppDbContext appDbContext, ICurrentUserService currentUserService) : IHotelAccessService
{
    public async Task<Result> EnsureCurrentUserManagesAsync(int hotelId, CancellationToken cancellationToken = default)
    {
        var userId = currentUserService.UserId;
        if (userId is null)
            return Result.Failure(AuthErrors.Unauthorized());
        var isManger = await appDbContext.HotelManagers.AnyAsync(h => h.HotelId == hotelId && h.UserId == userId,
            cancellationToken);
        if (!isManger)
            return Result.Failure(HotelErrors.NotManager());
        return Result.Success();
    }
}
