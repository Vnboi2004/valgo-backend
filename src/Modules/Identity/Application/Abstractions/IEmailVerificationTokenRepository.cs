using VAlgo.Modules.Identity.Domain.Entities;

namespace VAlgo.Modules.Identity.Application.Abstractions
{
    public interface IEmailVerificationTokenRepository
    {
        Task AddAsync(EmailVerificationToken token, CancellationToken cancellationToken = default);
        Task RemoveAsync(EmailVerificationToken token, CancellationToken cancellationToken = default);
        Task<EmailVerificationToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);
    }
}