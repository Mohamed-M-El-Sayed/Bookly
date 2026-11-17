using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.Hotels.Errors;
using Bookly.Domain.Entities.Hotels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Hotels.Commands.RemoveHotel;

public class RemoveHotelCommandHandler(IUnitOfWork unitOfWork,
    ILogger<RemoveHotelCommandHandler> logger)
    : IRequestHandler<RemoveHotelCommand, Result>
{
    public async Task<Result> Handle(RemoveHotelCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Removing hotel {HotelId}",
            request.HotelId);

        var hotel = await unitOfWork.Repository<Hotel>()
            .GetByIdAsync(request.HotelId, cancellationToken);

        if (hotel is null)
        {
            logger.LogWarning(
                "Cannot remove hotel. Hotel {HotelId} was not found",
                request.HotelId);

            return Result.Failure(HotelErrors.NotFound());
        }

        hotel.Delete();
        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation(
            "Hotel {HotelId} successfully removed",
            request.HotelId);
        return Result.Success();
    }
}
