using Bookly.Domain.Entities.Base;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;

namespace Bookly.Domain.Entities.SeasonalPricing;

public class SeasonalPrice : BaseEntity
{
    public int RoomTypeId { get; private set; }
    public string Name { get; private set; } = null!;
    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }
    public decimal PricePerNight { get; private set; }
    private SeasonalPrice() { }
    public SeasonalPrice(
        int roomTypeId,
        string name,
        DateOnly startDate,
        DateOnly endDate,
        decimal pricePerNight)
    {
        if (roomTypeId <= 0)
            throw new ArgumentOutOfRangeException(nameof(roomTypeId));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Name is required.",
                nameof(name));

        if (startDate > endDate)
            throw new ArgumentException(
                "Start date must be before or equal to end date.");

        if (pricePerNight <= 0)
            throw new ArgumentOutOfRangeException(nameof(pricePerNight));

        RoomTypeId = roomTypeId;
        Name = name;
        StartDate = startDate;
        EndDate = endDate;
        PricePerNight = pricePerNight;
    }

    public bool Covers(DateOnly night)
    {
        return StartDate <= night && night <= EndDate;
    }
}
