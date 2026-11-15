using Bookly.Application.Common.Models;
using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Hotels;
using Bookly.Application.Features.RoomTypes.Errors;
using Bookly.Application.Features.SeasonalPricing.Queries.GetSeasonalPriceById;
using Bookly.Application.Features.SeasonalPricing.Specifications;
using Bookly.Domain.Entities.RoomTypes;
using Bookly.Domain.Entities.SeasonalPricing;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.SeasonalPricing.Queries.GetSeasonalPricesByRoomTypeId;

public class GetSeasonalPricesByRoomTypeIdQueryHandler(IUnitOfWork unitOfWork,
    IHotelAccessService hotelAccessService,
    ILogger<GetSeasonalPricesByRoomTypeIdQueryHandler> logger)
    : IRequestHandler<GetSeasonalPricesByRoomTypeIdQuery, Result<PagedResult<SeasonalPriceResponse>>>
{
    public async Task<Result<PagedResult<SeasonalPriceResponse>>> Handle(GetSeasonalPricesByRoomTypeIdQuery request,
        CancellationToken cancellationToken)
    {
        var roomType = await unitOfWork.Repository<RoomType>()
            .GetByIdAsync(request.RoomTypeId, cancellationToken);

        if (roomType is null)
        {
            logger.LogWarning(
                "Room type {RoomTypeId} was not found",
                request.RoomTypeId);

            return Result<PagedResult<SeasonalPriceResponse>>
                .Failure(RoomTypeErrors.NotFound());
        }
        var accessResult = await hotelAccessService
            .EnsureCurrentUserManagesAsync(roomType.HotelId);

        if (accessResult.IsFailure)
            return Result<PagedResult<SeasonalPriceResponse>>
                .Failure(accessResult.Error!);

        var specification = new SeasonalPricesByRoomTypeIdSpecification(
            request.RoomTypeId,
            request.PageNumber,
            request.PageSize);

        var repository = unitOfWork.Repository<SeasonalPrice>();

        var seasonalPrices = await repository
            .GetAllWithSpecAsync(specification, cancellationToken);

        var totalCount = await repository.CountAsync(specification);

        var response = new PagedResult<SeasonalPriceResponse>(
            seasonalPrices.Adapt<IReadOnlyList<SeasonalPriceResponse>>(),
            totalCount,
            request.PageNumber,
            request.PageSize);

        logger.LogInformation(
            "Retrieved {Count} seasonal prices for room type {RoomTypeId}",
            response.Items.Count,
            request.RoomTypeId);

        return Result<PagedResult<SeasonalPriceResponse>>.Success(response);
    }
}