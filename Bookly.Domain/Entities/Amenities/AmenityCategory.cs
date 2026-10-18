using Bookly.Domain.Entities.Base;

namespace Bookly.Domain.Entities.Amenities;

public class AmenityCategory : BaseEntity
{
    public string NameEn { get; private set; } = null!;
    public string NameAr { get; private set; } = null!;
    public int SortOrder { get; private set; }
    public bool IsActive { get; private set; }
    private readonly List<Amenity> _amenities = new();
    public IReadOnlyCollection<Amenity> Amenities => _amenities.AsReadOnly();
    private AmenityCategory() { }

    public AmenityCategory(string nameEn, string nameAr, int sortOrder)
    {
        NameEn = nameEn.Trim();
        NameAr = nameAr.Trim();
        SortOrder = sortOrder;
        IsActive = true;
    }
    public void Update(string nameEn, string nameAr, int sortOrder)
    {
        NameEn = nameEn.Trim();
        NameAr = nameAr.Trim();
        SortOrder = sortOrder;
    }

    public void Deactivate() => IsActive = false;

    public void Activate() => IsActive = true;
}