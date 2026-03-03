using MediatR;
using VAlgo.Modules.Identity.Application.Abstractions.Persistence;
using VAlgo.Modules.Identity.Application.Abstractions.Security;
using VAlgo.Modules.Identity.Application.Commands.LoginUser;
using VAlgo.Modules.Identity.Application.Exceptions;
using VAlgo.Modules.Identity.Application.Persistence;
using VAlgo.Modules.Identity.Domain.Aggregates;
using VAlgo.SharedKernel.Time;

namespace VAlgo.Modules.Identity.Application.Commands.RefreshTokenUser
{
    public sealed class RefreshTokenUserCommandHandler : IRequestHandler<RefreshTokenUserCommand, LoginResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IAccessTokenGenerator _accessTokenGenerator;
        private readonly ISecureTokenGenerator _secureTokenGenerator;
        private readonly IClock _clock;
        private readonly IUnitOfWork _unitOfWork;

        public RefreshTokenUserCommandHandler(
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IAccessTokenGenerator accessTokenGenerator,
            ISecureTokenGenerator secureTokenGenerator,
            IClock clock,
            IUnitOfWork unitOfWork
        )
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _accessTokenGenerator = accessTokenGenerator;
            _secureTokenGenerator = secureTokenGenerator;
            _clock = clock;
            _unitOfWork = unitOfWork;
        }

        public async Task<LoginResult> Handle(RefreshTokenUserCommand request, CancellationToken cancellationToken)
        {
            var existingRefreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken);
            if (existingRefreshToken == null)
                throw new InvalidRefreshTokenException();

            if (existingRefreshToken.IsRevoked || existingRefreshToken.IsExpired(_clock))
                throw new InvalidRefreshTokenException();

            var user = await _userRepository.GetByIdAsync(existingRefreshToken.UserId, cancellationToken);
            if (user == null)
                throw new UserNotFoundException(existingRefreshToken.UserId.Value);

            existingRefreshToken.Revoke();

            var accessToken = _accessTokenGenerator.Generate(user);

            var newRefreshTokenValue = _secureTokenGenerator.Generate();

            var newRefreshTokenExpiresAt = _clock.UtcNow.AddDays(30);

            var newRefreshToken = RefreshToken.Create(user.Id, newRefreshTokenValue, newRefreshTokenExpiresAt);

            await _refreshTokenRepository.AddAsync(newRefreshToken, cancellationToken);

            return new LoginResult(user.Id.Value, accessToken, newRefreshTokenValue);
        }
    }
}