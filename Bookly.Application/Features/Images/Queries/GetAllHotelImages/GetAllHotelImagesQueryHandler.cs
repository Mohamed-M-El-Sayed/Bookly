using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.HotelImages.Specifications;
using Bookly.Application.Features.Hotels.Errors;
using Bookly.Domain.Entities.Hotels;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.HotelImages.Queries.GetAllHotelImages;

public class GetAllHotelImagesQueryHandler(
    IUnitOfWork unitOfWork,
    ILogger<GetAllHotelImagesQueryHandler> logger) : IRequestHandler<GetAllHotelImagesQuery, Result<List<HotelImagesResponse>>>
{
    public async Task<Result<List<HotelImagesResponse>>> Handle(GetAllHotelImagesQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all images for hotel ID {HotelId}", request.HotelId);

        var hotel = await unitOfWork.Repository<Hotel>()
            .GetByIdAsync(request.HotelId, cancellationToken);

        if (hotel is null)
        {
            logger.LogWarning("Hotel with ID {HotelId} not found", request.HotelId);
            return Result<List<HotelImagesResponse>>
                .Failure(HotelErrors.NotFound());
        }

        var images = await unitOfWork.Repository<HotelImage>()
            .GetAllWithSpecAsync(
                new HotelImagesByHotelIdSpecification(request.HotelId),
                cancellationToken);

        var response = images.Adapt<List<HotelImagesResponse>>();
        logger.LogInformation("Retrieved {ImageCount} images for hotel ID {HotelId}", 
            response.Count, request.HotelId);

        return Result<List<HotelImagesResponse>>
            .Success(response);
    }
}
