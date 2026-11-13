using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.Hotels.Errors;
using Bookly.Domain.Entities.Hotels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Hotels.Commands.AssignHotelManager;

public class AssignHotelManagerCommandHandler(IUnitOfWork unitOfWork,
    ILogger<AssignHotelManagerCommandHandler> logger)
    : IRequestHandler<AssignHotelManagerCommand, Result>
{
    public async Task<Result> Handle(AssignHotelManagerCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Assigning user {UserId} as manager of hotel {HotelId}",
            request.UserId,
            request.HotelId);

        var hotel = await unitOfWork.Repository<Hotel>()
            .GetByIdAsync(request.HotelId, cancellationToken);

        if (hotel is null)
        {
            logger.LogWarning(
                "Cannot assign manager. Hotel {HotelId} was not found",
                request.HotelId);

            return Result.Failure(HotelErrors.NotFound());
        }
        var alreadyAssigned = await unitOfWork.Repository<Hotel>().AnyAsync(h => h.Managers.Any(m => m.UserId == request.UserId), cancellationToken);
        if (alreadyAssigned)
        {
            logger.LogWarning(
                "User {UserId} is already assigned to a hotel",
                request.UserId);
            return Result.Failure(HotelErrors.ManagerAlreadyAssigned());
        }
        hotel.AddManager(request.UserId);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation(
            "User {UserId} successfully assigned to hotel {HotelId}",
            request.UserId,
            request.HotelId);
        return Result.Success();
    }
}