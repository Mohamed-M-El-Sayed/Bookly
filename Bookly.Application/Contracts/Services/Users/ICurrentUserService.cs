namespace Bookly.Application.Contracts.Services.Users;

public interface ICurrentUserService
{
    public Guid? UserId { get; }
}
