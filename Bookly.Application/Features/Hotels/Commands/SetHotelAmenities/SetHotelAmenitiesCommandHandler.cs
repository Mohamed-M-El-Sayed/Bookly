
using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Hotels;
using Bookly.Application.Features.Amenities.Messages;
using Bookly.Application.Features.Hotels.Errors;
using Bookly.Application.Features.Hotels.Specifications;
using Bookly.Domain.Entities.Amenities;
using Bookly.Domain.Entities.Hotels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Hotels.Commands.SetHotelAmenities;

public class SetHotelAmenitiesCommandHandler(
    IUnitOfWork unitOfWork,
    IHotelAccessService hotelAccessService,
    ILogger<SetHotelAmenitiesCommandHandler> logger)
    : IRequestHandler<SetHotelAmenitiesCommand, Result>
{
    public async Task<Result> Handle(
        SetHotelAmenitiesCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Setting amenities for hotel {HotelId}",
            request.HotelId);

        var hotel = await unitOfWork.Repository<Hotel>()
            .GetEntityWithSpecAsync(
                new HotelWithAmenitiesSpecification(request.HotelId),
                cancellationToken);

        if (hotel is null)
            return Result.Failure(HotelErrors.NotFound());

        var accessResult =
            await hotelAccessService.EnsureCurrentUserManagesAsync(
                request.HotelId,
                cancellationToken);

        if (accessResult.IsFailure)
            return accessResult;

        var ids = request.AmenityIds.Distinct().ToList();

        // Validate only newly added amenities.
        var currentIds = hotel.Amenities
            .Select(a => a.AmenityId)
            .ToHashSet();

        var addedIds = ids
            .Where(id => !currentIds.Contains(id))
            .ToList();

        if (addedIds.Count > 0)
        {
            var validCount = await unitOfWork.Repository<Amenity>()
                .CountAsync(
                    new AmenitiesForSelectionSpecification(addedIds),
                    cancellationToken);

            if (validCount != addedIds.Count)
                return Result.Failure(AmenityErrors.InvalidSelection());
        }
        hotel.SetAmenities(ids);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Successfully updated amenities for hotel {HotelId}. Count: {Count}",
            hotel.Id, ids.Count);
        return Result.Success();
    }
}
