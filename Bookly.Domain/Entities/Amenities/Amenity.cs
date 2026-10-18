using Bookly.Domain.Entities.Base;
using Bookly.Domain.Enums;

namespace Bookly.Domain.Entities.Amenities;

public class Amenity : BaseEntity
{
    public string NameEn { get; private set; } = null!;
    public string NameAr { get; private set; } = null!;
    public string? Icon { get; private set; }
    public int CategoryId { get; private set; }
    public AmenityCategory Category { get; private set; } = null!;
    public AmenityScope Scope { get; private set; }
    public bool IsActive { get; private set; }

    public bool AppliesToHotel => Scope is AmenityScope.Hotel or AmenityScope.Both;
    public bool AppliesToRoom => Scope is AmenityScope.Room or AmenityScope.Both;
    private Amenity() { }
    public Amenity(
        string nameEn,
        string nameAr,
        int categoryId,
        AmenityScope scope,
        string? icon)
    {
        NameEn = nameEn.Trim();
        NameAr = nameAr.Trim();
        CategoryId = categoryId;
        Scope = scope;
        Icon = string.IsNullOrWhiteSpace(icon) ? null : icon.Trim();
        IsActive = true;
    }

    public void Update(
        string nameEn,
        string nameAr,
        int categoryId,
        AmenityScope scope,
        string? icon)
    {
        NameEn = nameEn.Trim();
        NameAr = nameAr.Trim();
        CategoryId = categoryId;
        Scope = scope;
        Icon = string.IsNullOrWhiteSpace(icon) ? null : icon.Trim();
    }
    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}