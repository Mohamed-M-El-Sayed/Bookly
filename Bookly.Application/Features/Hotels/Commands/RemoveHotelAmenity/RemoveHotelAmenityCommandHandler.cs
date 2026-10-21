using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Hotels;
using Bookly.Application.Features.Hotels.Errors;
using Bookly.Application.Features.Hotels.Specifications;
using Bookly.Domain.Entities.Hotels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Hotels.Commands.RemoveHotelAmenity;

public class RemoveHotelAmenityCommandHandler(
    IUnitOfWork unitOfWork,
    IHotelAccessService hotelAccessService,
    ILogger<RemoveHotelAmenityCommandHandler> logger)
    : IRequestHandler<RemoveHotelAmenityCommand, Result>
{
    public async Task<Result> Handle(
        RemoveHotelAmenityCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Removing amenity {AmenityId} from hotel {HotelId}",
            request.AmenityId, request.HotelId);

        var hotel = await unitOfWork.Repository<Hotel>()
            .GetEntityWithSpecAsync(
                new HotelWithAmenitiesSpecification(request.HotelId), cancellationToken);
        if (hotel is null)
            return Result.Failure(HotelErrors.NotFound());

        var accessResult = await hotelAccessService.EnsureCurrentUserManagesAsync(
            hotel.Id,
            cancellationToken);

        if (accessResult.IsFailure)
            return accessResult;

        if (!hotel.RemoveAmenity(request.AmenityId))
        {
            logger.LogInformation(
                "Amenity {AmenityId} was not attached to hotel {HotelId}",
                request.AmenityId, hotel.Id);

            return Result.Success();
        }
        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation(
            "Amenity {AmenityId} removed from hotel {HotelId}",
            request.AmenityId, hotel.Id);
        return Result.Success();
    }
}