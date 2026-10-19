using Bookly.Application.Common.Results;
using Bookly.Application.Features.Amenities.Shared;
using MediatR;

namespace Bookly.Application.Features.Amenities.Queries.GetAmenities;

public record GetAmenitiesQuery(int CategoryId) : IRequest<Result<IReadOnlyList<AmenityResponse>>>;


