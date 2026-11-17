using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Services.Caching;
using MediatR;

namespace Bookly.Application.Features.Hotels.Commands.UpdateHotel;

public class UpdateHotelCommand : IRequest<Result<UpdateHotelResponse>>, ICacheInvalidationRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string DescriptionAr { get; set; } = null!;
    public int? StarRating { get; set; }
    public string Street { get; set; } = null!;
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Website { get; set; }
    public IReadOnlyCollection<string> Tags => [CacheTags.HotelSearch];

}
