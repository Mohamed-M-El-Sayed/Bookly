using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Users;
using Bookly.Application.Features.Amenities.Shared;
using Bookly.Application.Features.Amenities.Specification;
using Bookly.Domain.Entities.Amenities;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Amenities.Queries.GetAmenities;

public class GetAmenitiesQueryHandler(IUnitOfWork unitOfWork,
    ILogger<GetAmenitiesQueryHandler> logger,
    ICurrentUserService currentUserService) : IRequestHandler<GetAmenitiesQuery, Result<IReadOnlyList<AmenityResponse>>>
{
    public async Task<Result<IReadOnlyList<AmenityResponse>>> Handle(GetAmenitiesQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting amenities for category ID {CategoryId}.",
           request.CategoryId);
        var isAdmin = currentUserService.IsAdmin;
        var amenities = await unitOfWork.Repository<Amenity>()
            .GetAllWithSpecAsync(new AmenitiesSpecification(request.CategoryId, isAdmin));
        var response = amenities.Adapt<IReadOnlyList<AmenityResponse>>();
        return Result<IReadOnlyList<AmenityResponse>>.Success(response);
    }
}
