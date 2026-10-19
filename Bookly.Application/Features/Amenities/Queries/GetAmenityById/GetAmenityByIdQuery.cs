using Bookly.Application.Common.Results;
using Bookly.Application.Features.Amenities.Shared;
using MediatR;

namespace Bookly.Application.Features.Amenities.Queries.GetAmenityById;

public record GetAmenityByIdQuery(int Id) : IRequest<Result<AmenityResponse>>;

