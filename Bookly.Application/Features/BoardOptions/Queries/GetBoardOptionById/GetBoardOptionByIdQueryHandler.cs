using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Hotels;
using Bookly.Application.Features.BoardOptions.Errors;
using Bookly.Application.Features.BoardOptions.Queries.GetRoomTypeBoardOptions;
using Bookly.Application.Features.RoomTypes.Errors;
using Bookly.Domain.Entities.RoomTypes;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.BoardOptions.Queries.GetBoardOptionById;

public class GetBoardOptionByIdQueryHandler(IUnitOfWork unitOfWork,
    IHotelAccessService hotelAccessService,
    ILogger<GetBoardOptionByIdQueryHandler> logger)
    : IRequestHandler<GetBoardOptionByIdQuery, Result<BoardOptionResponse>>
{
    public async Task<Result<BoardOptionResponse>> Handle(GetBoardOptionByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Retrieving board option {BoardOptionId}",
            request.Id);
        var roomType = await unitOfWork.Repository<RoomType>().GetByIdAsync(request.RoomTypeId, cancellationToken);
        if (roomType is null)
            return Result<BoardOptionResponse>.Failure(RoomTypeErrors.NotFound());

        var accessResult = await hotelAccessService.EnsureCurrentUserManagesAsync(roomType.HotelId, cancellationToken);
        if (accessResult.IsFailure)
            return Result<BoardOptionResponse>.Failure(accessResult.Error!);

        var boardOption = await unitOfWork.Repository<BoardOption>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (boardOption is null)
        {
            logger.LogWarning(
                "Board option {BoardOptionId} was not found",
                request.Id);

            return Result<BoardOptionResponse>.Failure(BoardOptionErrors.NotFound());
        }

        var response = boardOption.Adapt<BoardOptionResponse>();
        logger.LogInformation(
            "Retrieved board option {BoardOptionId} successfully",
            request.Id);

        return Result<BoardOptionResponse>.Success(response);
    }
}