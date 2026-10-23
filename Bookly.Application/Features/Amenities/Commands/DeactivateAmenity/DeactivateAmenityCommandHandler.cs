using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.Amenities.Messages;
using Bookly.Domain.Entities.Amenities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Amenities.Commands.DeactivateAmenity;

public class DeactivateAmenityCommandHandler(
    IUnitOfWork unitOfWork,
    ILogger<DeactivateAmenityCommandHandler> logger) : IRequestHandler<DeactivateAmenityCommand, Result>
{
    public async Task<Result> Handle(DeactivateAmenityCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deactivating amenity {AmenityId}.", request.Id);
        var amenity = await unitOfWork.Repository<Amenity>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (amenity is null)
        {
            logger.LogWarning(
                "Cannot deactivate amenity {AmenityId}: amenity not found.",
                request.Id);
            return Result.Failure(AmenityErrors.NotFound());
        }
        amenity.Deactivate();
        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation(
            "Amenity {AmenityId} deactivated successfully.",
            amenity.Id);
        return Result.Success();
    }
}