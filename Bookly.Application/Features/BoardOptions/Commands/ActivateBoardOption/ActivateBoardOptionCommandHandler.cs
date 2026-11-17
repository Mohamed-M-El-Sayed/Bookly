using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Hotels;
using Bookly.Application.Features.BoardOptions.Errors;
using Bookly.Application.Features.RoomTypes.Errors;
using Bookly.Domain.Entities.RoomTypes;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.BoardOptions.Commands.ActivateBoardOption;

public class ActivateBoardOptionCommandHandler(IUnitOfWork unitOfWork,
    IHotelAccessService hotelAccessService,
    ILogger<ActivateBoardOptionCommandHandler> logger)
    : IRequestHandler<ActivateBoardOptionCommand, Result>
{
    public async Task<Result> Handle(ActivateBoardOptionCommand request, CancellationToken cancellationToken)
    {
        var roomType = await unitOfWork.Repository<RoomType>()
            .GetByIdAsync(request.RoomTypeId, cancellationToken);

        if (roomType is null)
            return Result.Failure(RoomTypeErrors.NotFound());

        var accessResult = await hotelAccessService
            .EnsureCurrentUserManagesAsync(roomType.HotelId);

        if (accessResult.IsFailure)
            return Result.Failure(accessResult.Error!);

        var boardOption = await unitOfWork.Repository<BoardOption>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (boardOption is null)
        {
            logger.LogWarning(
                "Board option {BoardOptionId} was not found",
                request.Id);

            return Result.Failure(BoardOptionErrors.NotFound());
        }

        boardOption.Activate();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Board option {BoardOptionId} was activated successfully",
            request.Id);

        return Result.Success();
    }
}