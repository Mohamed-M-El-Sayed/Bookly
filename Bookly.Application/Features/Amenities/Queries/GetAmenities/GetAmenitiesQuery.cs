using Bookly.Application.Common.Results;
using MediatR;

namespace Bookly.Application.Features.Amenities.Queries.GetAmenities;

public record GetAmenitiesQuery(int CategoryId) : IRequest<Result<IReadOnlyList<AmenityResponse>>>;


