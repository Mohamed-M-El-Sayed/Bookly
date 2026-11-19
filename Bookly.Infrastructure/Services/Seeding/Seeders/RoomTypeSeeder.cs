using Bookly.Domain.Entities.RoomTypes;
using Bookly.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookly.Infrastructure.Services.Seeding.Seeders;

public class RoomTypeSeeder(AppDbContext appDbContext)
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var hotels = await appDbContext.Hotels
            .Select(x => new { x.Id, x.Name })
            .ToListAsync(cancellationToken);

        var roomTypes = new List<RoomType>();

        foreach (var hotel in hotels)
        {
            roomTypes.AddRange(
            [
                RoomType.Create(
                    hotel.Id,
                    "Standard Room",
                    "غرفة قياسية",
                    50m,
                    2,
                    "A comfortable standard room.",
                    "غرفة قياسية مريحة.",
                    "City View",
                    "إطلالة على المدينة",
                    1,
                    25m),

                RoomType.Create(
                    hotel.Id,
                    "Deluxe Room",
                    "غرفة ديلوكس",
                    90m,
                    2,
                    "A spacious deluxe room with premium facilities.",
                    "غرفة ديلوكس واسعة مع مرافق مميزة.",
                    "Garden View",
                    "إطلالة على الحديقة",
                    2,
                    35m),

                RoomType.Create(
                    hotel.Id,
                    "Family Suite",
                    "جناح عائلي",
                    150m,
                    4,
                    "A spacious suite suitable for families.",
                    "جناح واسع مناسب للعائلات.",
                    "Sea View",
                    "إطلالة على البحر",
                    3,
                    60m)
            ]);
        }

        var existingRoomTypes = await appDbContext.RoomTypes
            .Select(x => new { x.HotelId, x.NameEn })
            .ToListAsync(cancellationToken);

        var newRoomTypes = roomTypes
            .Where(roomType => !existingRoomTypes.Any(existing =>
                existing.HotelId == roomType.HotelId &&
                existing.NameEn == roomType.NameEn))
            .ToList();

        if (newRoomTypes.Count == 0)
            return;

        await appDbContext.RoomTypes.AddRangeAsync(
            newRoomTypes,
            cancellationToken);

        await appDbContext.SaveChangesAsync(cancellationToken);
    }
}
