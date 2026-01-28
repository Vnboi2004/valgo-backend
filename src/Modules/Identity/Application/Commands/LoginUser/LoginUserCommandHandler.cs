using MediatR;
using VAlgo.Modules.Identity.Application.Abstractions;
using VAlgo.Modules.Identity.Application.Abstractions.Persistence;
using VAlgo.Modules.Identity.Application.Abstractions.Security;
using VAlgo.Modules.Identity.Application.Exceptions;
using VAlgo.Modules.Identity.Application.Policies;
using VAlgo.Modules.Identity.Domain.Aggregates;
using VAlgo.Modules.Identity.Domain.Enums;
using VAlgo.Modules.Identity.Domain.Exceptions;
using VAlgo.Modules.Identity.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;
using VAlgo.SharedKernel.Time;

namespace VAlgo.Modules.Identity.Application.Commands.LoginUser
{
    public sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ILoginAttemptRepository _loginAttemptRepository;
        private readonly ILoginPolicy _loginPolicy;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAccessTokenGenerator _accessTokenGenerator;
        private readonly ISecureTokenGenerator _secureTokenGenerator;
        private readonly IClock _clock;
        private readonly IUnitOfWork _unitOfWork;

        public LoginUserCommandHandler(
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            ILoginAttemptRepository loginAttemptRepository,
            ILoginPolicy loginPolicy,
            IPasswordHasher passwordHasher,
            IAccessTokenGenerator accessTokenGenerator,
            ISecureTokenGenerator secureTokenGenerator,
            IClock clock,
            IUnitOfWork unitOfWork
        )
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _loginAttemptRepository = loginAttemptRepository;
            _loginPolicy = loginPolicy;
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
            {
                var attempt = LoginAttempt.FailedByEmail(email, LoginFailureReason.InvalidCredentials, _clock);
                await RecordAttempt(attempt, cancellationToken);
                throw new InvalidCredentialsException();
            }

            try
            {
                await _loginPolicy.EvaluateAsync(user, _clock, cancellationToken);

            }
            catch (UserLockedException)
            {
                var attempt = LoginAttempt.FailedByUser(user.Id, LoginFailureReason.UserLocked, _clock);
                await RecordAttempt(attempt, cancellationToken);
                throw;
            }

            var verifyPassword = _passwordHasher.Verify(request.Password, user.PasswordHash.Value);
            if (!verifyPassword)
            {
                var attempt = LoginAttempt.FailedByUser(user.Id, LoginFailureReason.InvalidCredentials, _clock);
                await RecordAttempt(attempt, cancellationToken);
                throw new InvalidCredentialsException();
            }

            if (!user.IsEmailVerified)
            {
                var attempt = LoginAttempt.FailedByUser(user.Id, LoginFailureReason.EmailNotVerified, _clock);
                await RecordAttempt(attempt, cancellationToken);
                throw new EmailNotVerifiedException();
            }

            var successAttempt = LoginAttempt.Success(user.Id, _clock);

            await _loginAttemptRepository.AddAsync(successAttempt, cancellationToken);

            var accessToken = _accessTokenGenerator.Generate(user);

            var refreshTokenValue = _secureTokenGenerator.Generate();
            var refreshTokenExpiresAt = _clock.UtcNow.AddDays(30);

            var refreshToken = RefreshToken.Create(user.Id, refreshTokenValue, refreshTokenExpiresAt);

            await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new LoginResult(user.Id.Value, accessToken, refreshTokenValue);
        }

        private async Task RecordAttempt(LoginAttempt attempt, CancellationToken cancellationToken)
        {
            await _loginAttemptRepository.AddAsync(attempt, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}