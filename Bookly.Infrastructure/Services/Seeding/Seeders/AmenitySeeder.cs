using Bookly.Application.Contracts.Services.Seeding;
using Bookly.Domain.Entities.Amenities;
using Bookly.Domain.Enums;
using Bookly.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookly.Infrastructure.Services.Seeding.Seeders;

public class AmenitySeeder(AppDbContext appDbContext) : IDataSeeder
{

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var categories = await appDbContext.AmenityCategories
            .ToDictionaryAsync(
                x => x.NameEn,
                x => x.Id,
                cancellationToken);

        var amenities = new (string NameEn, string NameAr, string Category,
            AmenityScope Scope, string? Icon)[]
        {
            // General
            ("Free Wi-Fi", "واي فاي مجاني", "General", AmenityScope.Both, "wifi"),
            ("Air Conditioning", "تكييف هواء", "General", AmenityScope.Both, "air-vent"),
            ("Heating", "تدفئة", "General", AmenityScope.Both, "thermometer"),
            ("Free Parking", "موقف سيارات مجاني", "General", AmenityScope.Hotel, "car"),

            // Bathroom
            ("Private Bathroom", "حمام خاص", "Bathroom", AmenityScope.Room, "bath"),
            ("Shower", "دش", "Bathroom", AmenityScope.Room, "shower-head"),
            ("Bathtub", "حوض استحمام", "Bathroom", AmenityScope.Room, "bath"),
            ("Hair Dryer", "مجفف شعر", "Bathroom", AmenityScope.Room, "wind"),

            // Bedroom
            ("Wardrobe", "خزانة ملابس", "Bedroom", AmenityScope.Room, "door-closed"),
            ("Desk", "مكتب", "Bedroom", AmenityScope.Room, "lamp-desk"),
            ("Extra Bed", "سرير إضافي", "Bedroom", AmenityScope.Room, "bed"),

            // Kitchen
            ("Kitchen", "مطبخ", "Kitchen", AmenityScope.Room, "cooking-pot"),
            ("Refrigerator", "ثلاجة", "Kitchen", AmenityScope.Room, "refrigerator"),
            ("Microwave", "ميكروويف", "Kitchen", AmenityScope.Room, "microwave"),

            // Food & Drink
            ("Restaurant", "مطعم", "Food & Drink", AmenityScope.Hotel, "utensils"),
            ("Room Service", "خدمة الغرف", "Food & Drink", AmenityScope.Hotel, "concierge-bell"),
            ("Breakfast", "إفطار", "Food & Drink", AmenityScope.Hotel, "coffee"),

            // Entertainment
            ("Flat-screen TV", "تلفزيون بشاشة مسطحة", "Entertainment", AmenityScope.Room, "tv"),
            ("Swimming Pool", "حمام سباحة", "Entertainment", AmenityScope.Hotel, "waves"),
            ("Fitness Center", "مركز لياقة بدنية", "Entertainment", AmenityScope.Hotel, "dumbbell"),

            // Services
            ("24-hour Front Desk", "مكتب استقبال على مدار 24 ساعة", "Services", AmenityScope.Hotel, "clock"),
            ("Laundry Service", "خدمة غسيل الملابس", "Services", AmenityScope.Hotel, "washing-machine"),
            ("Airport Shuttle", "خدمة النقل من وإلى المطار", "Services", AmenityScope.Hotel, "bus"),

            // Outdoor
            ("Garden", "حديقة", "Outdoor", AmenityScope.Hotel, "trees"),
            ("Terrace", "تراس", "Outdoor", AmenityScope.Hotel, "sun"),
            ("Balcony", "شرفة", "Outdoor", AmenityScope.Room, "fence"),
        };

        var existingNames = await appDbContext.Amenities
            .Select(x => x.NameEn)
            .ToHashSetAsync(cancellationToken);

        foreach (var amenity in amenities)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (existingNames.Contains(amenity.NameEn))
                continue;

            if (!categories.TryGetValue(amenity.Category, out var categoryId))
            {
                throw new InvalidOperationException(
                    $"Amenity category '{amenity.Category}' was not found.");
            }

            appDbContext.Amenities.Add(new Amenity(
                amenity.NameEn,
                amenity.NameAr,
                categoryId,
                amenity.Scope,
                amenity.Icon));

            existingNames.Add(amenity.NameEn);
        }

        await appDbContext.SaveChangesAsync(cancellationToken);
    }
}