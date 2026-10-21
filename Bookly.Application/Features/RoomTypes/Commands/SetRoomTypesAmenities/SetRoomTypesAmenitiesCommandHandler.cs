using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Hotels;
using Bookly.Application.Features.Amenities.Messages;
using Bookly.Application.Features.Hotels.Specifications;
using Bookly.Application.Features.RoomTypes.Errors;
using Bookly.Application.Features.RoomTypes.Specifications;
using Bookly.Domain.Entities.Amenities;
using Bookly.Domain.Entities.RoomTypes;
using Bookly.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.RoomTypes.Commands.SetRoomTypesAmenities;

public class SetRoomTypesAmenitiesCommandHandler(IUnitOfWork unitOfWork,
    IHotelAccessService hotelAccessService,
    ILogger<SetRoomTypesAmenitiesCommandHandler> logger) : IRequestHandler<SetRoomTypesAmenitiesCommand, Result>
{
    public async Task<Result> Handle(SetRoomTypesAmenitiesCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting amenity assignment for RoomType {RoomTypeId}.", request.RoomTypeId);
        var roomType = await unitOfWork.Repository<RoomType>()
            .GetEntityWithSpecAsync(new RoomTypeWithAmenitiesSpecification(request.RoomTypeId), cancellationToken);
        if (roomType is null)
            return Result.Failure(RoomTypeErrors.NotFound());
        var result = await hotelAccessService.EnsureCurrentUserManagesAsync(roomType.HotelId, cancellationToken);
        if (result.IsFailure)
            return result;
        var ids = request.AmenityIds.ToList();
        var currentIds = roomType.Amenities.Select(r => r.AmenityId).ToList();
        var addedIds = ids.Where(id => !currentIds.Contains(id)).ToList();
        // validate on addedIds
        if (addedIds.Count > 0)
        {
            var validCount = await unitOfWork.Repository<Amenity>()
                .CountAsync(
                    new AmenitiesForSelectionSpecification(addedIds, AmenityScope.Room),
                    cancellationToken);

            if (validCount != addedIds.Count)
                return Result.Failure(AmenityErrors.InvalidSelection());
        }
        roomType.SetAmenities(ids);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Successfully updated amenities for hotel {HotelId}. Count: {Count}", request.RoomTypeId, ids.Count);
        return Result.Success();
    }
}
