using Bookly.Application.Common.Models;
using Bookly.Application.Common.Results;
using MediatR;
using System.Text.Json.Serialization;

namespace Bookly.Application.Features.RoomTypes.Queries.GetAvailableHotelRoomTypes;

public class GetAvailableHotelRoomTypesQuery : IRequest<Result<PagedResult<AvailableRoomTypeResponse>>>

{
    [JsonIgnore]
    public int HotelId { get; set; }
    public DateOnly CheckIn { get; set; }
    public DateOnly CheckOut { get; set; }
    public int Guests { get; set; }
    public int PageNumer { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}