using MediatR;
using VAlgo.Modules.Identity.Application.Abstractions;
using VAlgo.Modules.Identity.Application.Exceptions;
using VAlgo.Modules.Identity.Domain.Entities;
using VAlgo.Modules.Identity.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;
using VAlgo.SharedKernel.Time;

namespace VAlgo.Modules.Identity.Application.Commands.LoginUser
{
    public sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAccessTokenGenerator _accessTokenGenerator;
        ISecureTokenGenerator _secureTokenGenerator;
        private readonly IClock _clock;
        private readonly IUnitOfWork _unitOfWork;

        public LoginUserCommandHandler(
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IPasswordHasher passwordHasher,
            IAccessTokenGenerator accessTokenGenerator,
            ISecureTokenGenerator secureTokenGenerator,
            IClock clock,
            IUnitOfWork unitOfWork
        )
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _passwordHasher = passwordHasher;
            _accessTokenGenerator = accessTokenGenerator;
            _secureTokenGenerator = secureTokenGenerator;
            _clock = clock;
            _unitOfWork = unitOfWork;
        }

        public async Task<LoginResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var email = Email.Create(request.Email);

            var user = await _userRepository.GetByEmailAsync(email, cancellationToken);
            if (user == null)
                throw new InvalidCredentialsException();

            var verifyPassword = _passwordHasher.Verify(request.Password, user.PasswordHash.Value);
            if (!verifyPassword)
                throw new InvalidCredentialsException();

            if (!user.IsEmailVerified)
                throw new EmailNotVerifiedException();

            var accessToken = _accessTokenGenerator.Generate(user);

            var refreshTokenValue = _secureTokenGenerator.Generate();
            var refreshTokenExpiresAt = _clock.UtcNow.AddDays(30);

            var refreshToken = RefreshToken.Create(user.Id, refreshTokenValue, refreshTokenExpiresAt);

            await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new LoginResult(user.Id.Value, accessToken, refreshTokenValue);
        }
    }
}