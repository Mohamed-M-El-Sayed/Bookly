using Bookly.Domain.Entities.Amenities;

namespace Bookly.Domain.Entities.Hotels;

public class HotelAmenity
{
    public int HotelId { get; private set; }
    public int AmenityId { get; private set; }
    public Hotel Hotel { get; private set; } = default!;
    public Amenity Amenity { get; private set; } = default!;
    private HotelAmenity() { }
    public HotelAmenity(int hotelId, int amenityId)
    {
        HotelId = hotelId;
        AmenityId = amenityId;
    }
}
