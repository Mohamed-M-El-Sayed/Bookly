using Bookly.Application.Contracts.Services.Availability;
using Bookly.Domain.Entities.RoomTypes;
using Bookly.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookly.Infrastructure.Services.Availability
{
    public class AvailabilityInitializer(AppDbContext context) : IAvailabilityInitializer
    {
        private const int BatchSize = 50;
        private const int WindowDays = 90;
        public async Task<int> EnsureWindowAsync(DateOnly today, CancellationToken cancellationToken)
        {
            // Get all room type IDs.
            var roomTypeIds = await context.RoomTypes
                .AsNoTracking()
                .Select(r => r.Id)
                .ToListAsync(cancellationToken);

            var created = 0;

            // Process room types in batches to limit the amount of work per batch.
            foreach (var batch in roomTypeIds.Chunk(BatchSize))
            {
                // Ensure availability for the current batch.
                created += await EnsureWindowAsync(
                    batch,
                    today,
                    cancellationToken);
            }
            return created;
        }
        private async Task<int> EnsureWindowAsync(
        IReadOnlyCollection<int> roomTypeIds,
        DateOnly today,
        CancellationToken cancellationToken)
        {
            if (roomTypeIds.Count == 0)
                return 0;
            // Both today and lastDate are included
            var lastDate = today.AddDays(WindowDays - 1);
            // Get count for each room type
            var roomCounts = await context.Rooms
                .AsNoTracking()
                .Where(x => roomTypeIds.Contains(x.RoomTypeId))
                .GroupBy(x => x.RoomTypeId)
                .Select(group => new
                {
                    RoomTypeId = group.Key,
                    TotalRooms = group.Count()
                })
                .ToDictionaryAsync(
                    x => x.RoomTypeId,
                    x => x.TotalRooms,
                    cancellationToken);

            var existingDates = await context.RoomAvailabilities
                .AsNoTracking()
                .Where(x =>
                    roomTypeIds.Contains(x.RoomTypeId) &&
                    x.Date >= today &&
                    x.Date <= lastDate)
                .Select(x => new { x.RoomTypeId, x.Date })
                .ToListAsync(cancellationToken);

            var existingKeys = existingDates
                .Select(x => (x.RoomTypeId, x.Date))
                .ToHashSet();

            var missingRows = new List<RoomAvailability>();

            foreach (var roomTypeId in roomTypeIds)
            {
                var totalRooms = roomCounts.GetValueOrDefault(roomTypeId);

                for (var date = today; date <= lastDate; date = date.AddDays(1))
                {
                    // Skip dates that already have an record
                    if (existingKeys.Contains((roomTypeId, date)))
                        continue;
                    //  Create a record only for a missing date
                    missingRows.Add(
                        new RoomAvailability(roomTypeId, date, totalRooms));
                }
            }
            if (missingRows.Count == 0)
                return 0;
            context.RoomAvailabilities.AddRange(missingRows);
            await context.SaveChangesAsync(cancellationToken);
            return missingRows.Count;
        }
    }
}
