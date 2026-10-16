using Bookly.Application.Common.Results;
using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Features.BoardOptions.Specifications;
using Bookly.Application.Features.RoomTypes.Errors;
using Bookly.Domain.Entities.RoomTypes;
using MediatR;

namespace Bookly.Application.Features.BoardOptions.Commands.CreateBoardOption;

public class CreateBoardOptionCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateBoardOptionCommand, Result<CreateBoardOptionResponse>>
{
    public async Task<Result<CreateBoardOptionResponse>> Handle(CreateBoardOptionCommand request, CancellationToken cancellationToken)
    {
        var roomType = await unitOfWork.Repository<RoomType>()
             .GetEntityWithSpecAsync(new RoomTypeWithBoardOptionsSpecification(request.RoomTypeId), cancellationToken);
        if (roomType is null)
            return Result<CreateBoardOptionResponse>.Failure(RoomTypeErrors.NotFound());
        roomType.AddBoardOption(request.Type, request.ExtraPricePerNight);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<CreateBoardOptionResponse>.Success(new CreateBoardOptionResponse(request.RoomTypeId, request.Type));
    }
}
