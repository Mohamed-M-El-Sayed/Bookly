using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.Api.Controllers;

[Route("api/room-types/{roomTypeId:int}/seasonal-prices")]
[ApiController]
public class SeasonalPricesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public IActionResult get()
    {
        return Ok("hello");
    }

}
