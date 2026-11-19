using Bookly.Domain.Entities.Hotels;
using Bookly.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookly.Infrastructure.Services.Seeding.Seeders;

public class HotelSeeder(AppDbContext appDbContext)
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var hotels = new[]
        {
            Hotel.Create(
                "Nile Pearl Hotel",
                "A luxury hotel overlooking the Nile River, offering comfortable rooms and premium facilities.",
                "فندق فاخر يطل على نهر النيل، ويوفر غرفًا مريحة ومرافق مميزة.",
                5,
                new Address(
                    "Corniche El Nile",
                    "Cairo",
                    "Cairo",
                    "Egypt"),
                new ContactInfo(
                    "+201000000001",
                    "nilepearl@example.com",
                    "https://example.com/nilepearl")),

            Hotel.Create(
                "Pyramids View Resort",
                "A relaxing resort offering beautiful views of the Giza Pyramids and comfortable accommodation.",
                "منتجع مريح يوفر إطلالات رائعة على أهرامات الجيزة وإقامة مميزة.",
                5,
                new Address(
                    "Pyramids Road",
                    "Giza",
                    "Giza",
                    "Egypt"),
                new ContactInfo(
                    "+201000000002",
                    "pyramids@example.com",
                    "https://example.com/pyramids")),

            Hotel.Create(
                "Alexandria Sea View Hotel",
                "A seaside hotel with beautiful Mediterranean views and easy access to the beach.",
                "فندق ساحلي يتميز بإطلالات جميلة على البحر المتوسط وسهولة الوصول إلى الشاطئ.",
                4,
                new Address(
                    "Corniche Road",
                    "Alexandria",
                    "Alexandria",
                    "Egypt"),
                new ContactInfo(
                    "+201000000003",
                    "seaview@example.com",
                    "https://example.com/seaview")),

            Hotel.Create(
                "Luxor Royal Hotel",
                "A comfortable hotel located near Luxor's historical attractions, offering a memorable stay.",
                "فندق مريح بالقرب من المعالم التاريخية في الأقصر، يوفر إقامة مميزة.",
                4,
                new Address(
                    "Khaled Ibn El Walid Street",
                    "Luxor",
                    "Luxor",
                    "Egypt"),
                new ContactInfo(
                    "+201000000004",
                    "luxorroyal@example.com",
                    "https://example.com/luxor")),

            Hotel.Create(
                "Red Sea Paradise Resort",
                "A beachfront resort offering relaxing accommodation and easy access to Red Sea activities.",
                "منتجع على شاطئ البحر الأحمر يوفر إقامة مريحة وسهولة الوصول إلى الأنشطة البحرية.",
                5,
                new Address(
                    "Touristic Promenade",
                    "Hurghada",
                    "Red Sea",
                    "Egypt"),
                new ContactInfo(
                    "+201000000005",
                    "redsea@example.com",
                    "https://example.com/redsea"))
        };

        var existingNames = await appDbContext.Hotels
            .Select(x => x.Name)
            .ToHashSetAsync(cancellationToken);

        var newHotels = hotels
            .Where(h => !existingNames.Contains(h.Name))
            .ToList();

        if (newHotels.Count == 0)
            return;

        await appDbContext.Hotels.AddRangeAsync(
            newHotels,
            cancellationToken);

        await appDbContext.SaveChangesAsync(cancellationToken);
    }
}