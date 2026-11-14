using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Hotels;
using Bookly.Application.Features.RoomTypes.Errors;
using Bookly.Application.Features.SeasonalPricing.Errors;
using Bookly.Domain.Entities.RoomTypes;
using Bookly.Domain.Entities.SeasonalPricing;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.SeasonalPricing.Queries.GetSeasonalPriceById;

public class GetSeasonalPriceByIdQueryHandler(IUnitOfWork unitOfWork,
    IHotelAccessService hotelAccessService,
    ILogger<GetSeasonalPriceByIdQueryHandler> logger)
    : IRequestHandler<GetSeasonalPriceByIdQuery, Result<SeasonalPriceResponse>>
{
    public async Task<Result<SeasonalPriceResponse>> Handle(GetSeasonalPriceByIdQuery request, CancellationToken cancellationToken)
    {
        var seasonalPrice = await unitOfWork.Repository<SeasonalPrice>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (seasonalPrice is null)
        {
            logger.LogWarning(
                "Seasonal price {SeasonalPriceId} was not found",
                request.Id);

            return Result<SeasonalPriceResponse>.Failure(
                SeasonalPriceErrors.NotFound());
        }
        var roomType = await unitOfWork.Repository<RoomType>()
            .GetByIdAsync(seasonalPrice.RoomTypeId, cancellationToken);

        if (roomType is null)
            return Result<SeasonalPriceResponse>.Failure(RoomTypeErrors.NotFound());

        var accessResult = await hotelAccessService
            .EnsureCurrentUserManagesAsync(roomType.HotelId);

        if (accessResult.IsFailure)
            return Result<SeasonalPriceResponse>.Failure(accessResult.Error!);

        var response = seasonalPrice.Adapt<SeasonalPriceResponse>();
        logger.LogInformation("Seasonal price {SeasonalPriceId} retrieved successfully",
            request.Id);

        return Result<SeasonalPriceResponse>.Success(response);
    }
}