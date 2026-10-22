using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.Amenities.Messages;
using Bookly.Application.Features.Amenities.Specification;
using Bookly.Domain.Entities.Amenities;
using Bookly.Domain.Entities.Hotels;
using Bookly.Domain.Entities.RoomTypes;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Amenities.Commands.DeleteAmenity;

public class DeleteAmenityCommandHandler(IUnitOfWork unitOfWork,
    ILogger<DeleteAmenityCommandHandler> logger) : IRequestHandler<DeleteAmenityCommand, Result>
{
    public async Task<Result> Handle(DeleteAmenityCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting amenity {AmenityId}", request.AmenityId);
        var amenity = await unitOfWork.Repository<Amenity>().GetByIdAsync(request.AmenityId);
        if (amenity is null)
            return Result.Failure(AmenityErrors.NotFound());
        var usedByHotel = await unitOfWork.Repository<Hotel>().AnyAsync(
            new HotelsUsingAmenitySpecification(amenity.Id), cancellationToken);
        var usedByRoomType = !usedByHotel
            && await unitOfWork.Repository<RoomType>().AnyAsync(
                new RoomTypesUsingAmenitySpecification(amenity.Id), cancellationToken);
        if (usedByHotel || usedByRoomType)
        {
            logger.LogWarning(
                "Delete rejected: amenity {AmenityId} is used by a hotel or a room type", amenity.Id);
            return Result.Failure(AmenityErrors.InUse());
        }
        unitOfWork.Repository<Amenity>().Remove(amenity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Amenity {AmenityId} deleted", amenity.Id);
        return Result.Success();
    }
}
