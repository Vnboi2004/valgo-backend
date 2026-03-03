using VAlgo.Modules.Identity.Domain.Aggregates;

namespace VAlgo.Modules.Identity.Application.Abstractions.Persistence
{
    public interface IPasswordResetTokenRepository
    {
        Task AddAsync(PasswordResetToken token, CancellationToken cancellationToken = default);
        Task<PasswordResetToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);
    }
}