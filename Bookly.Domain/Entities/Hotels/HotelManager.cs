namespace Bookly.Domain.Entities.Hotels;

public class HotelManager
{
    public int HotelId { get; private set; }
    public Guid UserId { get; private set; }
    public Hotel Hotel { get; private set; } = default!;
    private HotelManager() { }
    public HotelManager(int hotelId, Guid userId)
    {
        HotelId = hotelId;
        UserId = userId;
    }
}
