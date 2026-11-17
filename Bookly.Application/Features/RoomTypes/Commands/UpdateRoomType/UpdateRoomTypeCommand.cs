using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Services.Caching;
using MediatR;
namespace Bookly.Application.Features.RoomTypes.Commands.UpdateRoomType;

public class UpdateRoomTypeCommand : IRequest<Result<UpdateRoomTypeResponse>>, ICacheInvalidationRequest
{
    public int Id { get; set; }
    public int HotelId { get; set; }
    public string Name { get; set; } = null!;
    public string NameAr { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string DescriptionAr { get; set; } = null!;
    public decimal BasePricePerNight { get; set; }
    public int MaxOccupancy { get; set; }
    public string View { get; set; } = null!;
    public string ViewAr { get; set; } = null!;
    public int BedCount { get; set; }
    public decimal SizeSqm { get; set; }
    public IReadOnlyCollection<string> Tags => [CacheTags.HotelSearch];
}

