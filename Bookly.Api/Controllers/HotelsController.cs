using Bookly.Application.Features.Hotels.Commands.AssignHotelManager;
using Bookly.Application.Features.Hotels.Commands.CreateHotel;
using Bookly.Application.Features.Hotels.Commands.RemoveHotel;
using Bookly.Application.Features.Hotels.Commands.RemoveHotelAmenity;
using Bookly.Application.Features.Hotels.Commands.RemoveHotelManager;
using Bookly.Application.Features.Hotels.Commands.SetHotelAmenities;
using Bookly.Application.Features.Hotels.Commands.UpdateHotel;
using Bookly.Application.Features.Hotels.Queries.GetHotelAmenities;
using Bookly.Application.Features.Hotels.Queries.GetHotelById;
using Bookly.Application.Features.Hotels.Queries.SearchHotels;
using Bookly.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.Api.Controllers;

[Route("api/hotels")]
[ApiController]
public class HotelsController(IMediator mediator) : ApiControllerBase
{

    /// <summary>
    /// Gets a hotel by its ID.
    /// </summary>
    [HttpGet("{hotelId:int}")]
    public async Task<IActionResult> GetHotelById(int hotelId, CancellationToken cancellationToken)
    {
        var query = new GetHotelByIdQuery(hotelId);
        var result = await mediator.Send(query, cancellationToken);
        return Respond(result);
    }

    /// <summary>
    /// Gets the active amenities assigned to a hotel.
    /// </summary>
    [HttpGet("{hotelId:int}/amenities")]
    public async Task<IActionResult> GetHotelAmenities([FromRoute] int hotelId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetHotelAmenitiesQuery(hotelId), cancellationToken);
        return Respond(result);
    }
    /// <summary>
    /// Creates a new hotel.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateHotelCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result);
    }

    /// <summary>
    /// Updates an existing hotel.
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateHotelCommand command, CancellationToken cancellationToken)
    {
        command.Id = id;
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result);
    }
    /// <summary>
    /// Deletes a hotel (Admin only).
    /// </summary>
    [HttpDelete("{hotelId:int}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> RemoveHotel(int hotelId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new RemoveHotelCommand(hotelId),
            cancellationToken);
        return Respond(result);
    }
    /// <summary>
    /// Searches for hotels based on the provided criteria.
    /// </summary>
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] SearchHotelsQuery query, CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(query, cancellationToken);
        return Respond(result);
    }
    /// <summary>
    ///  Sets the amenities for a hotel (Hotel Manager only).
    /// </summary>
    [HttpPost("{id:int}/amenities")]
    public async Task<IActionResult> SetHotelAmenities(int id, [FromBody] SetHotelAmenitiesCommand command, CancellationToken cancellationToken)
    {
        command.HotelId = id;
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result);
    }
    /// <summary>
    /// Removes one amenity from a hotel (Hotel manager only).
    /// </summary>
    [HttpDelete("{hotelId:int}/amenities/{amenityId:int}")]
    public async Task<IActionResult> RemoveAmenity(
        int hotelId, int amenityId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new RemoveHotelAmenityCommand(hotelId, amenityId), cancellationToken);
        return Respond(result);
    }
    /// <summary>
    /// Assigns a user as a manager of a hotel (Admin only).
    /// </summary>
    [HttpPost("managers/{userId:guid}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> AssignManager(int hotelId, Guid userId, CancellationToken cancellationToken)
    {
        var command = new AssignHotelManagerCommand(hotelId, userId);
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result, StatusCodes.Status204NoContent);
    }
    /// <summary>
    /// Removes a manager from a hotel (Admin only).
    /// </summary>
    [HttpDelete("managers/{userId:guid}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> RemoveManager(int hotelId, Guid userId, CancellationToken cancellationToken)
    {
        var command = new RemoveHotelManagerCommand(hotelId, userId);
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result, StatusCodes.Status204NoContent);
    }
}
