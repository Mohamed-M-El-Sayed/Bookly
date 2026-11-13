using FluentValidation;

namespace Bookly.Application.Features.Rooms.Queries.GetRoomTypeRooms;

public class GetRoomTypeRoomsQueryValidator : AbstractValidator<GetRoomTypeRoomsQuery>
{
    public GetRoomTypeRoomsQueryValidator()
    {
        RuleFor(x => x.RoomTypeId)
            .GreaterThan(0)
            .WithErrorCode("validation.roomtype.id.invalid")
            .WithMessage("Room type ID must be greater than zero.");

        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithErrorCode("Validation.Common.InvalidPageNumber")
            .WithMessage("Page number must be greater than zero.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(10, 50)
            .WithErrorCode("Validation.Common.InvalidPageSize")
            .WithMessage("Page size must be between 10 and 50.");
    }
}