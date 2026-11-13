using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Services.Availability;
using Bookly.Application.Features.Rooms.Errors;
using Bookly.Domain.Entities.RoomTypes;
using Bookly.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookly.Infrastructure.Services.Availability;

public class RoomAvailabilityService(AppDbContext _appDbContext) : IRoomAvailabilityService
{
    private const int WindowDays = 90;

    public async Task<Result> SyncRoomTypeAsync(int roomTypeId, DateOnly today, CancellationToken cancellationToken)
    {
        var lastDate = today.AddDays(WindowDays - 1);
        var totalRooms = await _appDbContext.Rooms
            .CountAsync(x => x.RoomTypeId == roomTypeId && x.IsActive, cancellationToken);
        var availabilities = await _appDbContext.RoomAvailabilities
           .Where(x =>
               x.RoomTypeId == roomTypeId &&
               x.Date >= today &&
               x.Date <= lastDate)
           .ToListAsync(cancellationToken);
        var conflict = availabilities
         .FirstOrDefault(x => x.ReservedRooms > totalRooms);
        if (conflict is not null)
        {
            return Result.Failure(
                RoomErrors.CannotReduceInventory(
                    conflict.Date));
        }

        var existingDates = availabilities
            .Select(x => x.Date)
            .ToHashSet();

        foreach (var availability in availabilities)
        {
            // Update the total rooms for existing availabilities
            availability.TrySetTotalRooms(totalRooms);
        }
        // add new availabilities for dates that don't exist yet
        var missingAvailabilities = new List<RoomAvailability>();

        for (var date = today; date <= lastDate; date = date.AddDays(1))
        {
            if (existingDates.Contains(date))
                continue;

            missingAvailabilities.Add(
                new RoomAvailability(roomTypeId, date, totalRooms));
        }

        await _appDbContext.RoomAvailabilities.AddRangeAsync(
            missingAvailabilities,
            cancellationToken);

        await _appDbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();

    }
}
