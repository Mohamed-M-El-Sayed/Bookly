using Bookly.Application.Contracts.Services.Users;
using Bookly.Domain.Constants;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Bookly.Infrastructure.Services.Authentication;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor)
    : ICurrentUserService
{
    private ClaimsPrincipal? CurrentUser =>
        httpContextAccessor.HttpContext?.User;

    public Guid? UserId
    {
        get
        {
            var userId = CurrentUser?.FindFirstValue(
                ClaimTypes.NameIdentifier)
                ?? CurrentUser?.FindFirstValue("sub");

            return Guid.TryParse(userId, out var id)
                ? id
                : null;
        }
    }

    public bool IsAdmin =>
        CurrentUser?.IsInRole(Roles.Admin) ?? false;
    public bool IsHotelManager =>
        CurrentUser?.IsInRole(Roles.Manager) == true;
}