using System.Text.Json.Serialization;
using Bookly.Application.Common.Results;
using Bookly.Domain.Enums;
using MediatR;

namespace Bookly.Application.Features.BoardOptions.Commands.UpdateBoardOption;

public class UpdateBoardOptionCommand : IRequest<Result>
{
    [JsonIgnore]
    public int RoomTypeId { get; set; }
    [JsonIgnore]
    public int BoardOptionId { get; set; }
    public BoardType Type { get; set; }
    public decimal ExtraPricePerNight { get; set; }
}
