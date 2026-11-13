using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Services.Hotels;
using Bookly.Application.Features.Images.Specifications;
using Bookly.Application.Features.RoomTypes.Errors;
using Bookly.Domain.Entities.RoomTypes;
using MediatR;

namespace Bookly.Application.Features.Images.Commands.AddRoomTypeImage;

public class AddRoomTypeImageCommandHandler(IUnitOfWork unitOfWork,
    IHotelAccessService hotelAccessService) : IRequestHandler<AddRoomTypeImageCommand, Result<AddRoomTypeImageResponse>>
{
    public async Task<Result<AddRoomTypeImageResponse>> Handle(AddRoomTypeImageCommand request, CancellationToken cancellationToken)
    {
        var roomType = await unitOfWork.Repository<RoomType>()
            .GetEntityWithSpecAsync(
                new RoomTypeByIdWithImagesSpecification(request.RoomTypeId), cancellationToken);
        if (roomType is null)
            return Result<AddRoomTypeImageResponse>.Failure(RoomTypeErrors.NotFound());
        var accessResult = await hotelAccessService.EnsureCurrentUserManagesAsync(roomType.HotelId, cancellationToken);
        if (!accessResult.IsSuccess)
            return Result<AddRoomTypeImageResponse>.Failure(accessResult.Error!);
        roomType.AddImage(new RoomTypeImage(request.RoomTypeId, request.Url, request.IsPrimary));
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<AddRoomTypeImageResponse>.Success(
            new AddRoomTypeImageResponse(request.RoomTypeId, request.Url, request.IsPrimary));
    }
}
