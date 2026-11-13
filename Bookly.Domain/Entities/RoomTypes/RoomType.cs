using Bookly.Domain.Entities.Base;
using Bookly.Domain.Entities.Hotels;
using Bookly.Domain.Entities.SeasonalPricing;
using Bookly.Domain.Enums;
using Bookly.Domain.Exceptions;

namespace Bookly.Domain.Entities.RoomTypes;

public class RoomType : BaseEntity
{
    public int HotelId { get; private set; }
    public string NameEn { get; private set; } = null!;
    public string NameAr { get; private set; } = null!;
    public decimal BasePricePerNight { get; private set; }
    public int MaxOccupancy { get; private set; }
    public string Description { get; private set; } = null!;
    public string DescriptionAr { get; private set; } = null!;
    public string View { get; private set; } = null!;
    public string ViewAr { get; private set; } = null!;
    public int BedCount { get; private set; }
    public decimal SizeSqm { get; private set; }
    public Hotel Hotel { get; private set; } = null!;
    private readonly List<Room> _rooms = new();
    public IReadOnlyCollection<Room> Rooms => _rooms.AsReadOnly();
    private readonly List<SeasonalPrice> _seasonalPrices = new();
    public IReadOnlyCollection<SeasonalPrice> SeasonalPrices => _seasonalPrices.AsReadOnly();
    private readonly List<BoardOption> _boardOptions = new();
    public IReadOnlyCollection<BoardOption> BoardOptions => _boardOptions.AsReadOnly();
    public IReadOnlyCollection<RoomAvailability> RoomAvailabilities => _roomAvailabilities.AsReadOnly();
    private readonly List<RoomAvailability> _roomAvailabilities = new();
    private readonly List<RoomTypeAmenity> _roomTypeAmenities = new();
    public IReadOnlyCollection<RoomTypeAmenity> Amenities => _roomTypeAmenities.AsReadOnly();
    private readonly List<RoomTypeImage> _images = new();
    public IReadOnlyCollection<RoomTypeImage> Images => _images.AsReadOnly();
    private RoomType()
    {

    }
    public static RoomType Create(
        int hotelId,
        string nameEn,
        string nameAr,
        decimal basePricePerNight,
        int maxOccupancy,
        string description,
        string descriptionAr,
        string view,
        string viewAr,
        int bedCount,
        decimal sizeSqm)
    {
        if (hotelId <= 0)
            throw new ArgumentOutOfRangeException(nameof(hotelId), "Hotel ID must be greater than zero.");

        if (string.IsNullOrWhiteSpace(nameEn))
            throw new ArgumentException("Room type name is required.", nameof(nameEn));

        if (string.IsNullOrWhiteSpace(nameAr))
            throw new ArgumentException("Arabic room type name is required.", nameof(nameAr));

        if (basePricePerNight <= 0)
            throw new ArgumentOutOfRangeException(nameof(basePricePerNight), "Price must be greater than zero.");

        if (maxOccupancy <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxOccupancy), "Max occupancy must be at least 1.");

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required.", nameof(description));

        if (string.IsNullOrWhiteSpace(descriptionAr))
            throw new ArgumentException("Arabic description is required.", nameof(descriptionAr));

        if (string.IsNullOrWhiteSpace(view))
            throw new ArgumentException("Room view is required.", nameof(view));

        if (string.IsNullOrWhiteSpace(viewAr))
            throw new ArgumentException("Arabic room view is required.", nameof(viewAr));

        if (bedCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(bedCount), "Bed count must be greater than zero.");

        if (sizeSqm <= 0)
            throw new ArgumentOutOfRangeException(nameof(sizeSqm), "Room size must be greater than zero.");

        return new RoomType
        {
            HotelId = hotelId,
            NameEn = nameEn,
            NameAr = nameAr,
            BasePricePerNight = basePricePerNight,
            MaxOccupancy = maxOccupancy,
            Description = description,
            DescriptionAr = descriptionAr,
            View = view,
            ViewAr = viewAr,
            BedCount = bedCount,
            SizeSqm = sizeSqm
        };
    }

    public void Update(
        string nameEn,
        string nameAr,
        string description,
        string descriptionAr,
        decimal basePricePerNight,
        int maxOccupancy,
        string view,
        string viewAr,
        int bedCount,
        decimal sizeSqm)
    {
        if (string.IsNullOrWhiteSpace(nameEn))
            throw new ArgumentException("Room type name is required.", nameof(nameEn));

        if (string.IsNullOrWhiteSpace(nameAr))
            throw new ArgumentException("Arabic room type name is required.", nameof(nameAr));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required.", nameof(description));

        if (string.IsNullOrWhiteSpace(descriptionAr))
            throw new ArgumentException("Arabic description is required.", nameof(descriptionAr));

        if (basePricePerNight <= 0)
            throw new ArgumentOutOfRangeException(nameof(basePricePerNight), "Price must be greater than zero.");

        if (maxOccupancy <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxOccupancy), "Max occupancy must be at least 1.");

        if (string.IsNullOrWhiteSpace(view))
            throw new ArgumentException("Room view is required.", nameof(view));

        if (string.IsNullOrWhiteSpace(viewAr))
            throw new ArgumentException("Arabic room view is required.", nameof(viewAr));

        if (bedCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(bedCount), "Bed count must be greater than zero.");

        if (sizeSqm <= 0)
            throw new ArgumentOutOfRangeException(nameof(sizeSqm), "Room size must be greater than zero.");

        NameEn = nameEn;
        NameAr = nameAr;
        Description = description;
        DescriptionAr = descriptionAr;
        BasePricePerNight = basePricePerNight;
        MaxOccupancy = maxOccupancy;
        View = view;
        ViewAr = viewAr;
        BedCount = bedCount;
        SizeSqm = sizeSqm;
    }
    public void AddSeasonalPrice(SeasonalPrice seasonalPrice)
    {
        if (seasonalPrice is null)
            throw new ArgumentNullException(nameof(seasonalPrice));
        var hasOverlap = _seasonalPrices.Any(x =>
            seasonalPrice.StartDate <= x.EndDate &&
            seasonalPrice.EndDate >= x.StartDate);

        if (hasOverlap)
            throw new DomainException(
                "Seasonal price dates overlap with an existing seasonal price.");

        _seasonalPrices.Add(seasonalPrice);
    }
    public void AddBoardOption(BoardType type, decimal extraPricePerNight)
    {
        if (_boardOptions.Any(b => b.Type == type))
            throw new DomainException($"Board option '{type}' already exists for this room type.");

        _boardOptions.Add(new BoardOption(Id, type, extraPricePerNight));
    }
    public void SetAmenities(IReadOnlyCollection<int> amenityIds)
    {
        var wanted = amenityIds.ToHashSet();
        _roomTypeAmenities.RemoveAll(
            a => !wanted.Contains(a.AmenityId));
        var existing = _roomTypeAmenities
            .Select(a => a.AmenityId)
            .ToHashSet();
        foreach (var id in wanted)
        {
            if (existing.Add(id))
                _roomTypeAmenities.Add(new RoomTypeAmenity(Id, id));
        }
    }
    public bool RemoveAmenity(int amenityId)
    {
        var roomTypeAmenity = _roomTypeAmenities.FirstOrDefault(x => x.AmenityId == amenityId);
        if (roomTypeAmenity is null)
            return false;
        _roomTypeAmenities.Remove(roomTypeAmenity);
        return true;
    }
    public bool TryReserve(DateOnly checkIn, DateOnly checkOut)
    {
        var nights = checkOut.DayNumber - checkIn.DayNumber;
        var availability = _roomAvailabilities
            .Where(x => x.Date >= checkIn && x.Date < checkOut)
            .ToList();
        if (nights <= 0 || availability.Count != nights)
            return false;
        if (availability.Any(x => x.AvailableRooms < 1))
            return false;
        foreach (var day in availability)
            day.TryReserve();

        return true;
    }
    public bool TryRelease(DateOnly checkIn, DateOnly checkOut)
    {
        var nights = checkOut.DayNumber - checkIn.DayNumber;
        var availability = _roomAvailabilities
            .Where(x => x.Date >= checkIn && x.Date < checkOut)
            .ToList();
        if (nights <= 0 || availability.Count != nights)
            return false;
        if (availability.Any(x => x.ReservedRooms <= 0))
            return false;
        foreach (var day in availability)
            day.TryRelease();
        return true;
    }
    public void AddImage(RoomTypeImage image)
    {
        ArgumentNullException.ThrowIfNull(image);
        if (image.IsPrimary)
        {
            foreach (var existing in _images)
                existing.UnsetPrimary();
        }
        _images.Add(image);
    }
    public decimal TotalPriceForStay(DateOnly checkIn, DateOnly checkOut, int? boardOptionId)
    {
        if (checkOut <= checkIn)
            throw new ArgumentException("Check-out must be after check-in.");
        var boardOption = boardOptionId.HasValue
           ? BoardOptions.FirstOrDefault(x => x.Id == boardOptionId.Value)
           : null;
        if (boardOptionId.HasValue && boardOption is null)
            throw new ArgumentException("Invalid board option.");
        decimal total = 0;
        for (var date = checkIn; date < checkOut; date = date.AddDays(1))
        {
            var seasonalPrice = _seasonalPrices.FirstOrDefault(
                x => x.StartDate <= date && x.EndDate >= date);
            total += seasonalPrice?.PricePerNight ?? BasePricePerNight;
            total += boardOption?.ExtraPricePerNight ?? 0;
        }
        return Math.Round(total, 2, MidpointRounding.AwayFromZero);
    }

}