using VAlgo.Modules.Identity.Domain.Exceptions;
using VAlgo.Modules.Identity.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;
using VAlgo.SharedKernel.Time;

namespace VAlgo.Modules.Identity.Domain.Aggregates
{
    public sealed class EmailVerificationToken : AggregateRoot<EmailVerificationTokenId>
    {
        public UserId UserId { get; private set; }
        public string Token { get; private set; }
        public DateTimeOffset ExpiresAt { get; private set; }
        public bool IsUsed { get; private set; }

        private EmailVerificationToken() { }

        private EmailVerificationToken(EmailVerificationTokenId id, UserId userId, string token, DateTimeOffset expiresAt)
            : base(id)
        {
            UserId = userId;
            Token = token;
            ExpiresAt = expiresAt;
            IsUsed = false;
        }

        public static EmailVerificationToken Create(UserId userId, string token, DateTimeOffset expiresAt)
            => new EmailVerificationToken(EmailVerificationTokenId.New(), userId, token, expiresAt);

        public void MarkAsUsed()
        {
            if (IsUsed)
                throw new VerificationTokenAlreadyUsedException();

            IsUsed = true;
        }

        public bool IsExpired(IClock clock)
            => clock.UtcNow >= ExpiresAt;
    }
}