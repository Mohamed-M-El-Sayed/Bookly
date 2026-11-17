using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.Hotels.Errors;
using Bookly.Domain.Entities.Hotels;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Hotels.Queries.GetHotelById;

public class GetHotelByIdQueryHandler(
    IUnitOfWork unitOfWork,
    ILogger<GetHotelByIdQueryHandler> logger)
    : IRequestHandler<GetHotelByIdQuery, Result<HotelResponse>>
{
    public async Task<Result<HotelResponse>> Handle(GetHotelByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Getting hotel with ID {HotelId}",
            request.HotelId);

        var hotel = await unitOfWork.Repository<Hotel>()
            .GetByIdAsync(request.HotelId, cancellationToken);
        if (hotel is null)
        {
            logger.LogWarning(
                "Hotel {HotelId} was not found",
                request.HotelId);

            return Result<HotelResponse>.Failure(HotelErrors.NotFound());
        }
        var response = hotel.Adapt<HotelResponse>();
        logger.LogInformation(
            "Hotel {HotelId} retrieved successfully",
            request.HotelId);
        return Result<HotelResponse>.Success(response);
    }
}