using Bookly.Application.Features.Favorites.Commands.AddFavorite;
using Bookly.Application.Features.Favorites.Commands.ClearMyFavorites;
using Bookly.Application.Features.Favorites.Commands.RemoveFavorite;
using Bookly.Application.Features.Favorites.Queries.GetMyFavorites;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FavoritesController(IMediator mediator) : ApiControllerBase
{
    /// <summary>
    /// Adds a hotel to the current user's favorites.
    /// </summary>
    [HttpPost()]
    public async Task<IActionResult> AddFavorite([FromBody] AddFavoriteCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result);
    }
    /// <summary>
    /// Get my current user's favorite hotels.
    /// </summary>
    [HttpGet("my-favorites")]
    [Authorize]
    public async Task<IActionResult> GetMyFavorites(
    CancellationToken cancellationToken)
    {
        var query = new GetMyFavoritesQuery();
        var result = await mediator.Send(query, cancellationToken);
        return Respond(result);
    }
    /// <summary>
    /// Removes a hotel from the current user's favorites.
    /// </summary>
    [HttpDelete("{hotelId:int}")]
    [Authorize]
    public async Task<IActionResult> RemoveFavorite(
    int hotelId,
    CancellationToken cancellationToken)
    {
        var command = new RemoveFavoriteCommand(hotelId);
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result);
    }
    /// <summary>
    /// Removes all favorites belonging to the current user.
    /// </summary>
    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> ClearMyFavorites(
        CancellationToken cancellationToken)
    {
        var command = new ClearMyFavoritesCommand();
        var result = await mediator.Send(command, cancellationToken);
        return Respond(result);
    }
}