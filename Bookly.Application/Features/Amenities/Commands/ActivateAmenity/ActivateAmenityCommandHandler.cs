using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.Amenities.Messages;
using Bookly.Domain.Entities.Amenities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Amenities.Commands.ActivateAmenity;

public class ActivateAmenityCommandHandler(IUnitOfWork unitOfWork,
    ILogger<ActivateAmenityCommandHandler> logger) : IRequestHandler<ActivateAmenityCommand, Result>
{
    public async Task<Result> Handle(ActivateAmenityCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Activating amenity {AmenityId}.", request.Id);
        var amenity = await unitOfWork.Repository<Amenity>()
           .GetByIdAsync(request.Id, cancellationToken);
        if (amenity is null)
        {
            logger.LogWarning(
                "Cannot activate amenity {AmenityId}: amenity not found.",
                request.Id);
            return Result.Failure(AmenityErrors.NotFound());
        }
        amenity.Activate();
        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation(
            "Amenity {AmenityId} activated successfully.", amenity.Id);
        return Result.Success();
    }
}
