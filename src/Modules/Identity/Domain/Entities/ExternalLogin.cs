using VAlgo.Modules.Identity.Domain.Enums;
using VAlgo.Modules.Identity.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Identity.Domain.Entities
{
    public sealed class ExternalLogin : Entity<ExternalLoginId>
    {
        public UserId UserId { get; private set; }
        public ExternalProvider Provider { get; private set; }
        public string ProviderUserId { get; private set; }

        private ExternalLogin() { }

        private ExternalLogin(ExternalLoginId id, UserId userId, ExternalProvider provider, string providerUserId)
            : base(id)
        {
            UserId = userId;
            Provider = provider;
            ProviderUserId = providerUserId;
        }

        public static ExternalLogin Create(UserId userId, ExternalProvider provider, string providerUserId)
            => new ExternalLogin(ExternalLoginId.New(), userId, provider, providerUserId);
    }
}