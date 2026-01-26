using VAlgo.Modules.Identity.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Identity.Domain.Aggregates
{
    public sealed class RefreshToken : Entity<RefreshTokenId>
    {
        public UserId UserId { get; private set; }
        public string Token { get; private set; }
        public DateTimeOffset ExpiresAt { get; private set; }
        public bool IsRevoked { get; private set; }

        private RefreshToken() { }

        private RefreshToken(RefreshTokenId id, UserId userId, string token, DateTimeOffset expiresAt) : base(id)
        {
            UserId = userId;
            Token = token;
            ExpiresAt = expiresAt;
            IsRevoked = false;
        }

        public static RefreshToken Create(UserId userId, string token, DateTimeOffset expiresAt)
            => new RefreshToken(RefreshTokenId.New(), userId, token, expiresAt);

        public void Revoke()
        {
            IsRevoked = true;
        }

        public bool IsExpired()
            => DateTimeOffset.UtcNow >= ExpiresAt;
    }
}