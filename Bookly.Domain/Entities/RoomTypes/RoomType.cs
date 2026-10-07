using Bookly.Domain.Entities.Base;
using Bookly.Domain.Entities.Hotels;
using Bookly.Domain.Entities.SeasonalPricing;
using Bookly.Domain.Enums;
using Bookly.Domain.Exceptions;

namespace Bookly.Domain.Entities.RoomTypes;

public class RoomType : BaseEntity
{
    public int HotelId { get; private set; }
    public string Name { get; private set; } = null!;
    public decimal BasePricePerNight { get; private set; }
    public int MaxOccupancy { get; private set; }
    public string Description { get; private set; } = null!;
    public string View { get; private set; } = null!;
    public Hotel Hotel { get; private set; }
    private readonly List<SeasonalPrice> _seasonalPrices = new();
    public IReadOnlyCollection<SeasonalPrice> SeasonalPrices => _seasonalPrices.AsReadOnly();
    private readonly List<BoardOption> _boardOptions = new();
    public IReadOnlyCollection<BoardOption> BoardOptions => _boardOptions.AsReadOnly();
    public IReadOnlyCollection<RoomAvailability> RoomAvailabilities => _roomAvailabilities.AsReadOnly();
    private readonly List<RoomAvailability> _roomAvailabilities = new();
    private RoomType()
    {

    }

    public static RoomType Create(int hotelId, string name, decimal price, int maxOccupancy, string description, string view)
    {
        if (price <= 0)
            throw new ArgumentException("Price must be greater than zero.");
        if (maxOccupancy <= 0)
            throw new ArgumentException("Max occupancy must be at least 1.");
        return new RoomType
        {
            HotelId = hotelId,
            Name = name,
            Description = description,
            BasePricePerNight = price,
            MaxOccupancy = maxOccupancy,
            View = view
        };
    }
    public void Update(string name, string description, decimal basePricePerNight, int maxOccupancy, string view)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Room type name is required.", nameof(name));

        if (basePricePerNight <= 0)
            throw new ArgumentOutOfRangeException(nameof(basePricePerNight), basePricePerNight, "Price must be greater than zero.");

        if (maxOccupancy <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxOccupancy), maxOccupancy, "Max occupancy must be at least 1.");

        Name = name;
        Description = description;
        BasePricePerNight = basePricePerNight;
        MaxOccupancy = maxOccupancy;
        View = view;
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
    public decimal PriceForNight(DateOnly night)
        => _seasonalPrices.FirstOrDefault(s => s.Covers(night))?.PricePerNight ?? BasePricePerNight;
}