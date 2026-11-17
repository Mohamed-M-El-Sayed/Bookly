using Bookly.Application.Common.Models;
using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.Rooms.Queries.GetRoomById;
using Bookly.Application.Features.Rooms.Specifications;
using Bookly.Domain.Entities.RoomTypes;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookly.Application.Features.Rooms.Queries.GetRoomTypeRooms;

public class GetRoomTypeRoomsQueryHandler(
    IUnitOfWork unitOfWork,
    ILogger<GetRoomTypeRoomsQueryHandler> logger)
    : IRequestHandler<GetRoomTypeRoomsQuery, Result<PagedResult<RoomResponse>>>
{
    public async Task<Result<PagedResult<RoomResponse>>> Handle(GetRoomTypeRoomsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Getting rooms for room type {RoomTypeId}.",
            request.RoomTypeId);
        var specification = new RoomTypeRoomsSpecification(
            request.RoomTypeId, request.PageNumber, request.PageSize);
        var rooms = await unitOfWork.Repository<Room>().GetAllWithSpecAsync(specification, cancellationToken);
        var totalCount = await unitOfWork.Repository<Room>().CountAsync(specification, cancellationToken);
        var response = rooms.Adapt<IReadOnlyList<RoomResponse>>();
        return Result<PagedResult<RoomResponse>>
            .Success(new PagedResult<RoomResponse>(response, totalCount, request.PageNumber, request.PageSize));
    }
}