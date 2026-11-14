using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Hotels;
using Bookly.Application.Features.RoomTypes.Errors;
using Bookly.Application.Features.SeasonalPricing.Errors;
using Bookly.Application.Features.SeasonalPricing.Specifications;
using Bookly.Domain.Entities.RoomTypes;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.SeasonalPricing.Commands.UpdateSeasonalPrice;

public class UpdateSeasonalPriceCommandHandler(IUnitOfWork unitOfWork,
    IHotelAccessService hotelAccessService,
    ILogger<UpdateSeasonalPriceCommandHandler> logger) : IRequestHandler<UpdateSeasonalPriceCommand, Result>
{
    public async Task<Result> Handle(UpdateSeasonalPriceCommand request, CancellationToken cancellationToken)
    {

        var roomType = await unitOfWork.Repository<RoomType>()
            .GetEntityWithSpecAsync(new RoomTypeWithSeasonalPricesSpecification(request.RoomTypeId), cancellationToken);

        if (roomType is null)
            return Result.Failure(RoomTypeErrors.NotFound());

        var accessResult = await hotelAccessService
            .EnsureCurrentUserManagesAsync(roomType.HotelId);

        if (accessResult.IsFailure)
            return Result.Failure(accessResult.Error!);

        var seasonalPriceExists = roomType.SeasonalPrices
            .Any(x => x.Id == request.Id);

        if (!seasonalPriceExists)
        {
            logger.LogWarning(
                "Seasonal price {SeasonalPriceId} was not found for room type {RoomTypeId}",
                request.Id,
                request.RoomTypeId);

            return Result.Failure(SeasonalPriceErrors.NotFound());
        }
        roomType.UpdateSeasonalPrice(roomType.SeasonalPrices.FirstOrDefault(s => s.Id == request.Id)!);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Seasonal price {SeasonalPriceId} was updated successfully",
            request.Id);

        return Result.Success();

    }
}