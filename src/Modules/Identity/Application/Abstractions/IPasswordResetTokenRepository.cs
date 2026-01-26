using VAlgo.Modules.Identity.Domain.Entities;

namespace VAlgo.Modules.Identity.Application.Abstractions
{
    public interface IPasswordResetTokenRepository
    {
        Task AddAsync(PasswordResetToken token, CancellationToken cancellationToken = default);
        Task<PasswordResetToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);
    }
}