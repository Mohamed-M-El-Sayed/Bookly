using Bookly.Domain.Entities.Base;

namespace Bookly.Domain.Entities.Identity;

public class PasswordHistory : BaseEntity
{
    public Guid UserId { get; private set; }
    public string PasswordHash { get; private set; } = default!;
    public PasswordHistory(Guid userId, string passwordHash)
    {
        UserId = userId;
        PasswordHash = passwordHash;
    }
}
