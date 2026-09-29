using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
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
            return Result<CreateRoomTypeResponse>.Failure(Errors.NotFound("Hotel not found"));
        var roomType = RoomType.Create(
            request.HotelId,
            request.Name,
            request.BasePricePerNight,
            request.MaxOccupancy,
            request.Description,
            request.View);
        await unitOfWork.Repository<RoomType>().AddAsync(roomType, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        var response = new CreateRoomTypeResponse(roomType.Id, roomType.Name);
        return Result<CreateRoomTypeResponse>.Success(response);
    }
}
