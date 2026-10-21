using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Hotels.Queries.GetHotelAmenities;

public record GetHotelAmenitiesQuery(int HotelId) : IRequest<Result<IReadOnlyList<HotelAmenityResponse>>>;
public record HotelAmenityResponse(int HotelId,
    int AmenityId,
    string NameAr,
    string NameEn,
    string? Icon,
    string Scope,
    int CategoryId,
    string CategoryNameEn,
    string CategoryNameAr,
    bool IsActive);
