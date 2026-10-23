using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Hotels;
using Bookly.Application.Contracts.Services.Users;
using Bookly.Application.Features.RoomTypes.Errors;
using Bookly.Application.Features.RoomTypes.Specifications;
using Bookly.Domain.Entities.RoomTypes;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.RoomTypes.Queries.GetRoomTypeAmenities;

public class GetRoomTypeAmenitiesQueryHandler(IUnitOfWork unitOfWork,
    IHotelAccessService hotelAccessService,
    ICurrentUserService currentUserService,
    ILogger<GetRoomTypeAmenitiesQueryHandler> logger)
    : IRequestHandler<GetRoomTypeAmenitiesQuery, Result<IReadOnlyList<RoomTypeAmenityResponse>>>
{
    public async Task<Result<IReadOnlyList<RoomTypeAmenityResponse>>> Handle(
        GetRoomTypeAmenitiesQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting amenities for room type {RoomTypeId}", request.RoomTypeId);

        var roomType = await unitOfWork.Repository<RoomType>()
            .GetByIdAsync(request.RoomTypeId, cancellationToken);
        if (roomType is null)
        {
            return Result<IReadOnlyList<RoomTypeAmenityResponse>>
                .Failure(RoomTypeErrors.NotFound());
        }

        var includeInactive = false;

        if (currentUserService.IsHotelManager)
        {
            // The manager must manage the hotel that owns this room type.
            var accessResult = await hotelAccessService
                .EnsureCurrentUserManagesAsync(
                    roomType.HotelId,
                    cancellationToken);

            if (accessResult.IsFailure)
            {
                return Result<IReadOnlyList<RoomTypeAmenityResponse>>
                    .Failure(accessResult.Error!);
            }

            includeInactive = true;
        }

        var roomTypeAmenities = await unitOfWork.Repository<RoomTypeAmenity>()
            .GetAllWithSpecAsync(
                new RoomTypeAmenitiesSpecification(
                    request.RoomTypeId,
                    includeInactive),
                cancellationToken);

        var response =
            roomTypeAmenities.Adapt<IReadOnlyList<RoomTypeAmenityResponse>>();

        return Result<IReadOnlyList<RoomTypeAmenityResponse>>
            .Success(response);
    }
}