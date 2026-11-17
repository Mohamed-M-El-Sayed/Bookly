using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.Images.Specifications;
using Bookly.Application.Features.RoomTypes.Errors;
using Bookly.Domain.Entities.RoomTypes;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Images.Queries.GetAllRoomTypeImages;

public class GetAllRoomTypeImagesQueryHandler(
    IUnitOfWork unitOfWork,
    ILogger<GetAllRoomTypeImagesQueryHandler> logger) : IRequestHandler<GetAllRoomTypeImagesQuery, Result<List<RoomTypeImagesResponse>>>
{
    public async Task<Result<List<RoomTypeImagesResponse>>> Handle(
        GetAllRoomTypeImagesQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all images for room type ID {RoomTypeId}", request.RoomTypeId);
        var roomType = await unitOfWork.Repository<RoomType>()
            .GetByIdAsync(request.RoomTypeId, cancellationToken);
        if (roomType is null)
        {
            logger.LogWarning("Room type with ID {RoomTypeId} not found", request.RoomTypeId);
            return Result<List<RoomTypeImagesResponse>>
                .Failure(RoomTypeErrors.NotFound());
        }
        var images = await unitOfWork.Repository<RoomTypeImage>()
            .GetAllWithSpecAsync(new RoomTypeImagesByRoomTypeIdSpecification(request.RoomTypeId),
                cancellationToken);
        var response = images.Adapt<List<RoomTypeImagesResponse>>();
        logger.LogInformation("Retrieved {ImageCount} images for room type ID {RoomTypeId}",
            response.Count, request.RoomTypeId);
        return Result<List<RoomTypeImagesResponse>>
            .Success(response);
    }
}
