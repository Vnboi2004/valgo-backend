using VAlgo.Modules.Identity.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;
using VAlgo.SharedKernel.Time;

namespace VAlgo.Modules.Identity.Domain.Entities
{
    public sealed class PasswordResetToken : Entity<PasswordResetTokenId>
    {
        public UserId UserId { get; private set; }
        public string Token { get; private set; }
        public DateTimeOffset ExpiresAt { get; private set; }
        public bool IsUsed { get; private set; }

        private PasswordResetToken() { }

        private PasswordResetToken(PasswordResetTokenId id, UserId userId, string token, DateTimeOffset expiresAt) : base(id)
        {
            UserId = userId;
            Token = token;
            ExpiresAt = expiresAt;
            IsUsed = false;
        }

        public static PasswordResetToken Create(UserId userId, string token, DateTimeOffset expiresAt)
            => new(PasswordResetTokenId.New(), userId, token, expiresAt);

        public void MarkAsUsed()
        {
            IsUsed = true;
        }

        public bool IsExpired(IClock clock)
            => clock.UtcNow >= ExpiresAt;
    }
}