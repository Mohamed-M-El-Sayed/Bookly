using Bookly.Application.Features.RoomTypes.Commands.CreateRoomType;
using Bookly.Application.Features.RoomTypes.Commands.RemoveRoomTypeAmenity;
using Bookly.Application.Features.RoomTypes.Commands.SetRoomTypesAmenities;
using Bookly.Application.Features.RoomTypes.Queries.GetRoomTypeAmenities;
using Bookly.Application.Features.RoomTypes.Queries.GetRoomTypeById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.Api.Controllers;

[Route("api/hotels/{hotelId}/room-types")]
[ApiController]
public class RoomTypesController(IMediator mediator) : ApiControllerBase
{

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int hotelId, int id)
    {
        var result = await mediator.Send(new GetRoomTypeByIdQuery(hotelId, id));
        return Respond(result);
    }
    /// <summary>
    /// Creates a new room type for the specified hotel.
    /// </summary>
    [HttpPost]
    // user  
    public async Task<IActionResult> Create(int hotelId, [FromBody] CreateRoomTypeCommand command, CancellationToken cancellationToken)
    {
        command.HotelId = hotelId;
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result);
    }
    /// <summary>
    /// Sets the amenities for a room types (Hotel Manager only).
    /// </summary>
    [HttpPost("{id:int}/amenities")]
    public async Task<IActionResult> SetRoomTypeAmenities([FromRoute] int id, [FromBody] SetRoomTypesAmenitiesCommand command, CancellationToken cancellationToken)
    {
        command.RoomTypeId = id;
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result);
    }
    /// <summary>
    /// Removes one amenity from a room type (Hotel manager only).
    /// </summary>
    [HttpDelete("{id:int}/amenities/{amenityId:int}")]
    public async Task<IActionResult> RemoveAmenity(
        int id, int amenityId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new RemoveRoomTypeAmenityCommand(id, amenityId), cancellationToken);
        return Respond(result);
    }
    /// <summary>
    /// Gets the amenities assigned to a room type.
    /// </summary>
    [HttpGet("{roomTypeId:int}/amenities")]
    public async Task<IActionResult> GetAmenities(
        int roomTypeId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetRoomTypeAmenitiesQuery(roomTypeId),
            cancellationToken);
        return Respond(result);
    }
}

