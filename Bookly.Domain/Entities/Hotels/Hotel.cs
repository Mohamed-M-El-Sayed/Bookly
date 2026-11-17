using Bookly.Domain.Entities.Base;
using Bookly.Domain.Entities.Cancellation;
using Bookly.Domain.Entities.RoomTypes;

namespace Bookly.Domain.Entities.Hotels;

public class Hotel : SoftDeletableEntity
{
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string DescriptionAr { get; private set; } = null!;
    public int? StarRating { get; private set; }
    public Address Address { get; private set; } = null!;
    public ContactInfo ContactInfo { get; private set; } = null!;
    private readonly List<RoomType> _roomTypes = new();
    public IReadOnlyCollection<RoomType> RoomTypes => _roomTypes.AsReadOnly();
    private readonly List<HotelAmenity> _hotelAmenities = new();
    public IReadOnlyCollection<HotelAmenity> Amenities => _hotelAmenities.AsReadOnly();
    private readonly List<HotelImage> _images = new();
    public IReadOnlyCollection<HotelImage> Images => _images.AsReadOnly();
    private readonly List<HotelManager> _hotelManagers = new();
    public IReadOnlyCollection<HotelManager> Managers => _hotelManagers;
    public CancellationPolicy CancellationPolicy { get; private set; } = null!;
    private Hotel() { }

    private Hotel(
        string name,
        string description,
        string descriptionAr,
        int? starRating,
        Address address,
        ContactInfo contactInfo)
    {
        Name = name;
        Description = description;
        DescriptionAr = descriptionAr;
        StarRating = starRating;
        Address = address;
        ContactInfo = contactInfo;
    }

    public static Hotel Create(
        string name,
        string description,
        string descriptionAr,
        int? starRating,
        Address address,
        ContactInfo contactInfo)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Hotel name is required.", nameof(name));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException(
                "Description is required.", nameof(description));

        if (string.IsNullOrWhiteSpace(descriptionAr))
            throw new ArgumentException(
                "Arabic description is required.", nameof(descriptionAr));

        if (starRating is < 1 or > 5)
            throw new ArgumentOutOfRangeException(
                nameof(starRating), starRating,
                "Star rating must be between 1 and 5.");

        ArgumentNullException.ThrowIfNull(address);
        ArgumentNullException.ThrowIfNull(contactInfo);

        return new Hotel(
            name,
            description,
            descriptionAr,
            starRating,
            address,
            contactInfo);
    }

    public void Update(
        string name,
        string description,
        string descriptionAr,
        int? starRating,
        Address address,
        ContactInfo contactInfo)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Hotel name is required.", nameof(name));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException(
                "Description is required.", nameof(description));

        if (string.IsNullOrWhiteSpace(descriptionAr))
            throw new ArgumentException(
                "Arabic description is required.", nameof(descriptionAr));

        if (starRating is < 1 or > 5)
            throw new ArgumentOutOfRangeException(
                nameof(starRating), starRating,
                "Star rating must be between 1 and 5.");

        ArgumentNullException.ThrowIfNull(address);
        ArgumentNullException.ThrowIfNull(contactInfo);

        Name = name;
        Description = description;
        DescriptionAr = descriptionAr;
        StarRating = starRating;
        Address = address;
        ContactInfo = contactInfo;
    }
    public void SetAmenities(IReadOnlyCollection<int> amenityIds)
    {
        var wanted = amenityIds.ToHashSet();
        _hotelAmenities.RemoveAll(
            h => !wanted.Contains(h.AmenityId));
        var existing = _hotelAmenities
            .Select(h => h.AmenityId)
            .ToHashSet();
        foreach (var id in wanted)
        {
            if (existing.Add(id))
                _hotelAmenities.Add(new HotelAmenity(Id, id));
        }
    }
    public bool RemoveAmenity(int amenityId)
    {
        var hotelAmenity = _hotelAmenities.FirstOrDefault(x => x.AmenityId == amenityId);
        if (hotelAmenity is null)
            return false;
        _hotelAmenities.Remove(hotelAmenity);
        return true;
    }
    public void AddImage(HotelImage image)
    {
        ArgumentNullException.ThrowIfNull(image);
        if (image.IsPrimary)
        {
            foreach (var existing in _images)
                existing.UnsetPrimary();
        }
        _images.Add(image);
    }
    public void AddManager(Guid userId)
    {
        _hotelManagers.Add(new HotelManager(Id, userId));
    }
}