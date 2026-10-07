using Bookly.Domain.Entities.Base;

namespace Bookly.Domain.Entities.Identity
{
    public class RefreshToken : BaseEntity
    {
        public string Token { get; private set; } = null!;
        public DateTime ExpiresAt { get; private set; }
        public bool IsRevoked { get; private set; }
        public Guid UserId { get; private set; }
        public bool IsExpired => ExpiresAt <= DateTime.UtcNow;
        public bool IsActive => !IsRevoked && !IsExpired;
        private RefreshToken(){ }
        public RefreshToken(string token, DateTime expiresAt)
        {
            Token = token;
            ExpiresAt = expiresAt;
        }
        public void Revoke()
        {
            IsRevoked = true;
        }
    }
}
