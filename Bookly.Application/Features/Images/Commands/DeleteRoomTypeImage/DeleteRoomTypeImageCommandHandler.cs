using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Files;
using Bookly.Application.Contracts.Services.Hotels;
using Bookly.Application.Features.Images.Errors;
using Bookly.Application.Features.RoomTypes.Errors;
using Bookly.Domain.Entities.RoomTypes;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Images.Commands.DeleteRoomTypeImage;

public class DeleteRoomTypeImageCommandHandler(IUnitOfWork unitOfWork,
    IHotelAccessService hotelAccessService,
    IFileService fileService,
    ILogger<DeleteRoomTypeImageCommandHandler> logger) : IRequestHandler<DeleteRoomTypeImageCommand, Result>
{
    public async Task<Result> Handle(DeleteRoomTypeImageCommand request, CancellationToken cancellationToken)
    {
        var roomType = await unitOfWork.Repository<RoomType>()
            .GetByIdAsync(request.RoomTypeId, cancellationToken);

        if (roomType is null)
            return Result.Failure(RoomTypeErrors.NotFound());
        var accessResult = await hotelAccessService
            .EnsureCurrentUserManagesAsync(roomType.HotelId);

        if (accessResult.IsFailure)
            return accessResult;

        var image = await unitOfWork.Repository<RoomTypeImage>()
            .GetByIdAsync(request.ImageId, cancellationToken);

        if (image is null || image.RoomTypeId != request.RoomTypeId)
            return Result.Failure(ImageErrors.RoomTypeImageNotFound());

        unitOfWork.Repository<RoomTypeImage>().Remove(image);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await fileService.DeleteFileAsync(image.Url, cancellationToken);
        logger.LogInformation(
            "Room type image {ImageId} was deleted from room type {RoomTypeId}",
            request.ImageId,
            request.RoomTypeId);

        return Result.Success();
    }
}
