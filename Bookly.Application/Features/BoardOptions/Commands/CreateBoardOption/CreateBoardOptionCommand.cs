using System.Text.Json.Serialization;
using Bookly.Application.Common.Results;
using Bookly.Domain.Enums;
using MediatR;

namespace Bookly.Application.Features.BoardOptions.Commands.CreateBoardOption;

public class CreateBoardOptionCommand : IRequest<Result<CreateBoardOptionResponse>>
{
    [JsonIgnore]
    public int RoomTypeId { get; set; }
    public BoardType Type { get; set; }
    public decimal ExtraPricePerNight { get; set; }
}
