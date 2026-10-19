using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.Amenities.Messages;
using Bookly.Application.Features.Amenities.Shared;
using Bookly.Domain.Entities.Amenities;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Amenities.Queries.GetAmenityById;

public class GetAmenityByIdQueryHandler(IUnitOfWork unitOfWork,
    ILogger<GetAmenityByIdQueryHandler> logger) : IRequestHandler<GetAmenityByIdQuery, Result<AmenityResponse>>
{
    public async Task<Result<AmenityResponse>> Handle(GetAmenityByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting amenity {AmenityId}", request.Id);
        var amenity = await unitOfWork.Repository<Amenity>().GetByIdAsync(request.Id, cancellationToken);
        if (amenity is null)
            return Result<AmenityResponse>.Failure(AmenityErrors.NotFound());
        var response = amenity.Adapt<AmenityResponse>();
        logger.LogInformation("Amenity {AmenityId} retrieved successfully", amenity.Id);
        return Result<AmenityResponse>.Success(response);
    }
}
