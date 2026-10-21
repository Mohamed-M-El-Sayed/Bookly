using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Hotels;
using Bookly.Application.Features.RoomTypes.Errors;
using Bookly.Application.Features.RoomTypes.Specifications;
using Bookly.Domain.Entities.RoomTypes;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.RoomTypes.Commands.RemoveRoomTypeAmenity;

public class RemoveRoomTypeAmenityCommandHandler(
    IUnitOfWork unitOfWork,
    IHotelAccessService hotelAccessService,
    ILogger<RemoveRoomTypeAmenityCommandHandler> logger)
    : IRequestHandler<RemoveRoomTypeAmenityCommand, Result>
{
    public async Task<Result> Handle(
        RemoveRoomTypeAmenityCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Removing amenity {AmenityId} from room type {RoomTypeId}",
            request.AmenityId, request.RoomTypeId);

        var roomType = await unitOfWork.Repository<RoomType>()
            .GetEntityWithSpecAsync(
                new RoomTypeWithAmenitiesSpecification(request.RoomTypeId),
                cancellationToken);

        if (roomType is null)
            return Result.Failure(RoomTypeErrors.NotFound());

        var accessResult = await hotelAccessService.EnsureCurrentUserManagesAsync(
            roomType.HotelId,
            cancellationToken);

        if (accessResult.IsFailure)
            return accessResult;

        if (!roomType.RemoveAmenity(request.AmenityId))
        {
            logger.LogInformation(
                "Amenity {AmenityId} was not attached to room type {RoomTypeId}",
                request.AmenityId, roomType.Id);

            return Result.Success();
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation(
            "Amenity {AmenityId} removed from room type {RoomTypeId}",
            request.AmenityId, roomType.Id);
        return Result.Success();
    }
}