namespace Bookly.Application.Contracts.Services.Users;

public interface ICurrentUserService
{
    Guid? UserId { get; }
    bool IsAdmin { get; }
    bool IsHotelManager { get; }

}
