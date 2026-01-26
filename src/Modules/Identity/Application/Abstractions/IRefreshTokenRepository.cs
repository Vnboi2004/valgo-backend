using VAlgo.Modules.Identity.Domain.Entities;
using VAlgo.Modules.Identity.Domain.ValueObjects;

namespace VAlgo.Modules.Identity.Application.Abstractions
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
        Task RemoveAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
        Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);
        Task RevokeAllAsync(UserId userId, CancellationToken cancellationToken = default);
    }
}