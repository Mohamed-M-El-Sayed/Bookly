using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Files;
using Bookly.Application.Contracts.Services.Hotels;
using Bookly.Application.Features.Hotels.Errors;
using Bookly.Application.Features.Images.Errors;
using Bookly.Domain.Entities.Hotels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Images.Commands.DeleteHotelImage;

public class DeleteHotelImageCommandHandler(IUnitOfWork unitOfWork,
    IHotelAccessService hotelAccessService,
    IFileService fileService,
    ILogger<DeleteHotelImageCommandHandler> logger) : IRequestHandler<DeleteHotelImageCommand, Result>
{
    public async Task<Result> Handle(DeleteHotelImageCommand request, CancellationToken cancellationToken)
    {
        var hotel = await unitOfWork.Repository<Hotel>().GetByIdAsync(request.HotelId, cancellationToken);
        if (hotel is null)
            return Result.Failure(HotelErrors.NotFound());
        var accessResult = await hotelAccessService.EnsureCurrentUserManagesAsync(request.HotelId);
        if (accessResult.IsFailure)
            return accessResult;

        var image = await unitOfWork.Repository<HotelImage>()
            .GetByIdAsync(request.ImageId, cancellationToken);

        if (image is null || image.HotelId != request.HotelId)
            return Result.Failure(ImageErrors.HotelImageNotFound());
        unitOfWork.Repository<HotelImage>().Remove(image);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        await fileService.DeleteFileAsync(image.Url, cancellationToken);

        logger.LogInformation(
            "Hotel image {ImageId} was deleted from hotel {HotelId}",
            request.ImageId,
            request.HotelId);
        return Result.Success();
    }
}
