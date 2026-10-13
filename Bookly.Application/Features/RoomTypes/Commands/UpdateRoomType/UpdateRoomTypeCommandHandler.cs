using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.RoomTypes.Errors;
using Bookly.Domain.Entities.RoomTypes;
using MediatR;
namespace Bookly.Application.Features.RoomTypes.Commands.UpdateRoomType;

public class UpdateRoomTypeCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateRoomTypeCommand, Result<UpdateRoomTypeResponse>>
{
    public async Task<Result<UpdateRoomTypeResponse>> Handle(UpdateRoomTypeCommand request, CancellationToken cancellationToken)
    {
        var roomType = await unitOfWork.Repository<RoomType>().GetByIdAsync(request.Id, cancellationToken);
        if (roomType is null)
            return Result<UpdateRoomTypeResponse>.Failure(RoomTypeErrors.NotFound());
        roomType.Update(
            request.Name,
            request.NameAr,
            request.Description,
            request.DescriptionAr,
            request.BasePricePerNight,
            request.MaxOccupancy,
            request.View,
            request.ViewAr,
            request.BedCount,
            request.SizeSqm);
        unitOfWork.Repository<RoomType>().Update(roomType);
        await unitOfWork.SaveChangesAsync();
        var response = new UpdateRoomTypeResponse(request.Id, request.Name, request.NameAr);
        return Result<UpdateRoomTypeResponse>.Success(response);
    }
}
