using Bookly.Application.Common.Results;
using MediatR;
using System.Text.Json.Serialization;

namespace Bookly.Application.Features.Amenities.Commands.Categories.UpdateAmenity;

public class UpdateAmenityCategoryCommand : IRequest<Result>
{
    [JsonIgnore]
    public int CategoryId { get; set; }
    public string NameEn { get; set; } = null!;
    public string NameAr { get; set; } = null!;
    public int SortOrder { get; set; }
}