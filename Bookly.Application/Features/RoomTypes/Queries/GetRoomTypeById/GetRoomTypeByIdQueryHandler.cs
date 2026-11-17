using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.RoomTypes.Errors;
using Bookly.Domain.Entities.RoomTypes;
using Mapster;
using MediatR;
namespace Bookly.Application.Features.RoomTypes.Queries.GetRoomTypeById;

public class GetRoomTypeByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetRoomTypeByIdQuery, Result<RoomTypeResponse>>
{
    public async Task<Result<RoomTypeResponse>> Handle(GetRoomTypeByIdQuery request, CancellationToken cancellationToken)
    {
        var roomType = await unitOfWork.Repository<RoomType>().GetByIdAsync(request.Id, cancellationToken);
        if (roomType is null)
            return Result<RoomTypeResponse>.Failure(RoomTypeErrors.NotFound());
        var response = roomType.Adapt<RoomTypeResponse>();
        return Result<RoomTypeResponse>.Success(response);
    }
}
