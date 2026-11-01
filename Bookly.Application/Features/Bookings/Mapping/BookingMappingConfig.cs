using Bookly.Application.Features.Bookings.Queries.GetBookingById;
using Bookly.Domain.Entities.Bookings;
using Mapster;

namespace Bookly.Application.Features.Bookings.Mapping;

internal class BookingMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Booking, BookingDetailsResponse>()
            .Map(dest => dest.Room.RoomTypeId, src => src.BookingRooms.FirstOrDefault()!.RoomTypeId)
            .Map(dest => dest.Room.RoomTypeName, src => src.BookingRooms.FirstOrDefault()!.RoomType.NameEn)
            .Map(dest => dest.Room.BoardOptionId, src => src.BookingRooms.FirstOrDefault()!.BoardOptionId);
    }
}
