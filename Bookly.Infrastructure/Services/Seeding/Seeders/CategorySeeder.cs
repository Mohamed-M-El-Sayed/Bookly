using Bookly.Domain.Entities.Amenities;
using Bookly.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookly.Infrastructure.Services.Seeding.Seeders;

public class CategorySeeder(AppDbContext appDbContext)
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var categories = new (string NameEn, string NameAr, int SortOrder)[]
        {
            ("General", "عام", 1),
            ("Bathroom", "الحمام", 2),
            ("Bedroom", "غرفة النوم", 3),
            ("Kitchen", "المطبخ", 4),
            ("Food & Drink", "الطعام والشراب", 5),
            ("Entertainment", "الترفيه", 6),
            ("Services", "الخدمات", 7),
            ("Outdoor", "المرافق الخارجية", 8),
        };

        foreach (var category in categories)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var exists = await appDbContext.AmenityCategories
                .AnyAsync(
                    x => x.NameEn == category.NameEn,
                    cancellationToken);

            if (exists)
                continue;

            appDbContext.AmenityCategories.Add(new AmenityCategory(
                    category.NameEn,
                    category.NameAr,
                    category.SortOrder));
        }
        await appDbContext.SaveChangesAsync(cancellationToken);
    }
}
