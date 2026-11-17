using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Hotels;
using Bookly.Application.Features.RoomTypes.Errors;
using Bookly.Application.Features.SeasonalPricing.Errors;
using Bookly.Domain.Entities.RoomTypes;
using Bookly.Domain.Entities.SeasonalPricing;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.SeasonalPricing.Commands.DeleteSeasonalPrice;

public class DeleteSeasonalPriceCommandHandler(IUnitOfWork unitOfWork,
    IHotelAccessService hotelAccessService,
    ILogger<DeleteSeasonalPriceCommandHandler> logger) : IRequestHandler<DeleteSeasonalPriceCommand, Result>
{
    public async Task<Result> Handle(DeleteSeasonalPriceCommand request, CancellationToken cancellationToken)
    {
        var seasonalPrice = await unitOfWork.Repository<SeasonalPrice>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (seasonalPrice is null)
        {
            logger.LogWarning(
                "Seasonal price {SeasonalPriceId} was not found",
                request.Id);
            return Result.Failure(SeasonalPriceErrors.NotFound());
        }

        var roomType = await unitOfWork.Repository<RoomType>()
            .GetByIdAsync(seasonalPrice.RoomTypeId, cancellationToken);

        if (roomType is null)
        {
            logger.LogWarning(
                "Room type {RoomTypeId} was not found for seasonal price {SeasonalPriceId}",
                seasonalPrice.RoomTypeId,
                request.Id);
            return Result.Failure(RoomTypeErrors.NotFound());
        }

        var accessResult = await hotelAccessService
            .EnsureCurrentUserManagesAsync(roomType.HotelId);

        if (accessResult.IsFailure)
            return Result.Failure(accessResult.Error!);

        unitOfWork.Repository<SeasonalPrice>().Remove(seasonalPrice);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation(
            "Seasonal price {SeasonalPriceId} deleted successfully",
            request.Id);

        return Result.Success();
    }
}