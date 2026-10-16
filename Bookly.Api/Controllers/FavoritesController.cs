using Bookly.Application.Features.Favorites.Commands.AddFavorite;
using MediatR;
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
}