using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.RoomTypes.Commands.UpdateRoomType;

public class UpdateRoomTypeCommand
    : IRequest<Result<UpdateRoomTypeResponse>>
{
    public int Id { get; set; }
    public int HotelId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal BasePricePerNight { get; set; }
    public int MaxOccupancy { get; set; }
    public string View { get; set; } = null!;
}


