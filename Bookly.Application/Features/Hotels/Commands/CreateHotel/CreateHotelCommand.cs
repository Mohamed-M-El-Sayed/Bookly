using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Hotels.Commands.CreateHotel;

public class CreateHotelCommand : IRequest<Result<CreateHotelResponse>>
{
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
}
