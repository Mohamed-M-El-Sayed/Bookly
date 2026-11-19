using Bookly.Application.Contracts.Services.Seeding;
using Bookly.Infrastructure.Services.Seeding.Seeders;

namespace Bookly.Infrastructure.Services.Seeding;

public class DatabaseSeeder(RoleSeeder roleSeeder,
    AdminUserSeeder adminUserSeeder,
    CategorySeeder categorySeeder,
    AmenitySeeder amenitySeeder,
    HotelSeeder hotelSeeder,
    RoomTypeSeeder roomTypeSeeder) : IDataSeeder
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await roleSeeder.SeedAsync(cancellationToken);
        await adminUserSeeder.SeedAsync(cancellationToken);
        await categorySeeder.SeedAsync(cancellationToken);
        await amenitySeeder.SeedAsync(cancellationToken);
        await hotelSeeder.SeedAsync(cancellationToken);
        await roomTypeSeeder.SeedAsync(cancellationToken);
    }
}
