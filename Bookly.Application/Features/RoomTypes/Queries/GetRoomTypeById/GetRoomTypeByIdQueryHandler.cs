using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Domain.Entities.RoomTypes;
using Mapster;
using MediatR;
namespace Bookly.Application.Features.RoomTypes.Queries.GetRoomTypeById;

public class GetRoomTypeByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetRoomTypeByIdQuery, Result<GetRoomTypeByIdResponse>>
{
    public async Task<Result<GetRoomTypeByIdResponse>> Handle(GetRoomTypeByIdQuery request, CancellationToken cancellationToken)
    {
        var roomType = await unitOfWork.Repository<RoomType>().GetByIdAsync(request.Id, cancellationToken);
        if (roomType is null)
            return Result<GetRoomTypeByIdResponse>.Failure(Errors.NotFound("Room Type not found"));
        var response = roomType.Adapt<GetRoomTypeByIdResponse>();
        return Result<GetRoomTypeByIdResponse>.Success(response);
    }
}
