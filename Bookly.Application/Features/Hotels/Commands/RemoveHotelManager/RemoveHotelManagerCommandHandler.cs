using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.Hotels.Errors;
using Bookly.Application.Features.Hotels.Specifications;
using Bookly.Domain.Entities.Hotels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Hotels.Commands.RemoveHotelManager;

public class RemoveHotelManagerCommandHandler(IUnitOfWork unitOfWork,
    ILogger<RemoveHotelManagerCommandHandler> logger)
    : IRequestHandler<RemoveHotelManagerCommand, Result>
{
    public async Task<Result> Handle(RemoveHotelManagerCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Removing user {UserId} from hotel {HotelId} managers",
            request.UserId,
            request.HotelId);

        var hotel = await unitOfWork.Repository<Hotel>()
            .GetByIdAsync(request.HotelId, cancellationToken);

        if (hotel is null)
        {
            logger.LogWarning(
                "Cannot remove manager. Hotel {HotelId} was not found",
                request.HotelId);

            return Result.Failure(HotelErrors.NotFound());
        }

        var hotelManager = await unitOfWork.Repository<HotelManager>()
            .GetEntityWithSpecAsync(new HotelManagerByIdSpecification(request.HotelId,
            request.UserId), cancellationToken);
        if (hotelManager is null)
        {
            logger.LogWarning(
                "User {UserId} is not assigned to hotel {HotelId}",
                request.UserId,
                request.HotelId);

            return Result.Failure(HotelErrors.ManagerNotAssigned());
        }
        unitOfWork.Repository<HotelManager>().Remove(hotelManager);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation(
            "User {UserId} successfully removed from hotel {HotelId}",
            request.UserId,
            request.HotelId);

        return Result.Success();
    }
}