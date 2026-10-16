using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.Hotels.Errors;
using Bookly.Domain.Entities.Hotels;
using Bookly.Domain.Entities.RoomTypes;
using MediatR;

namespace Bookly.Application.Features.RoomTypes.Commands.CreateRoomType;

public class CreateRoomTypeCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateRoomTypeCommand, Result<CreateRoomTypeResponse>>
{
    public async Task<Result<CreateRoomTypeResponse>> Handle(CreateRoomTypeCommand request, CancellationToken cancellationToken)
    {
        var hotel = await unitOfWork.Repository<Hotel>().GetByIdAsync(request.HotelId);
        if (hotel is null)
            return Result<CreateRoomTypeResponse>.Failure(HotelErrors.NotFound());
        var roomType = RoomType.Create(
            request.HotelId,
            request.Name,
            request.NameAr,
            request.BasePricePerNight,
            request.MaxOccupancy,
            request.Description,
            request.DescriptionAr,
            request.View,
            request.ViewAr,
            request.BedCount,
            request.SizeSqm);
        await unitOfWork.Repository<RoomType>().AddAsync(roomType, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        var response = new CreateRoomTypeResponse(roomType.Id, roomType.Name, roomType.NameAr);
        return Result<CreateRoomTypeResponse>.Success(response);
    }
}
