using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Hotels;
using Bookly.Application.Features.BoardOptions.Queries.GetRoomTypeBoardOptions;
using Bookly.Application.Features.RoomTypes.Errors;
using Bookly.Domain.Entities.RoomTypes;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.RoomTypes.Queries.GetRoomTypeBoardOptions;

public class GetRoomTypeBoardOptionsQueryHandler(IUnitOfWork unitOfWork,
    IHotelAccessService hotelAccessService,
    ILogger<GetRoomTypeBoardOptionsQueryHandler> logger)
    : IRequestHandler<GetRoomTypeBoardOptionsQuery, Result<List<BoardOptionResponse>>>
{
    public async Task<Result<List<BoardOptionResponse>>> Handle(GetRoomTypeBoardOptionsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving board options for room type {RoomTypeId}", request.RoomTypeId);
        var roomType = await unitOfWork.Repository<RoomType>()
            .GetByIdAsync(request.RoomTypeId, cancellationToken);
        if (roomType is null)
            return Result<List<BoardOptionResponse>>.Failure(RoomTypeErrors.NotFound());

        var accessResult = await hotelAccessService.EnsureCurrentUserManagesAsync(roomType.HotelId, cancellationToken);
        if (accessResult.IsFailure)
            return Result<List<BoardOptionResponse>>.Failure(accessResult.Error!);

        var spec = new BoardOptionsByRoomTypeIdSpecification(request.RoomTypeId);

        var boardOptions = await unitOfWork.Repository<BoardOption>()
            .GetAllWithSpecAsync(spec, cancellationToken);

        if (boardOptions is null)
            return Result<List<BoardOptionResponse>>.Failure(
                RoomTypeErrors.NotFound());
        var response = boardOptions.Adapt<List<BoardOptionResponse>>();
        return Result<List<BoardOptionResponse>>.Success(response);
    }
}