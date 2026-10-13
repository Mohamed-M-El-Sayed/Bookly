using Bookly.Api.Extensions;
using Bookly.Api.Resources;
using Bookly.Application.Common.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Bookly.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
        private IStringLocalizer<SharedResource>? _localizer;
        protected IStringLocalizer<SharedResource> Localizer =>
       _localizer ??= HttpContext.RequestServices
           .GetRequiredService<IStringLocalizer<SharedResource>>();

        protected IActionResult Respond(Result result, int successStatus = StatusCodes.Status204NoContent)
            => result.ToResponse(successStatus, Localizer);

        protected IActionResult Respond<T>(Result<T> result, int successStatus = StatusCodes.Status200OK)
            => result.ToResponse(successStatus, Localizer);
    }
}
