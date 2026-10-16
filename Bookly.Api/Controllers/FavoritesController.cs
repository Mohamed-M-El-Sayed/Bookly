using Bookly.Application.Features.Favorites.Commands.AddFavorite;
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
}