using MediatR;
using VAlgo.Modules.Identity.Application.Abstractions;
using VAlgo.Modules.Identity.Application.Exceptions;
using VAlgo.Modules.Identity.Domain.Aggregates;
using VAlgo.Modules.Identity.Domain.Entities;
using VAlgo.Modules.Identity.Domain.Exceptions;
using VAlgo.Modules.Identity.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;
using VAlgo.SharedKernel.Time;

namespace VAlgo.Modules.Identity.Application.Commands.RegisterUser
{
    public sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailVerificationTokenRepository _emailVerificationTokenRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ISecureTokenGenerator _secureTokenGenerator;
        private readonly IEmailSender _emailSender;
        private readonly IClock _clock;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterUserCommandHandler(
            IUserRepository userRepository,
            IEmailVerificationTokenRepository emailVerificationTokenRepository,
            IPasswordHasher passwordHasher,
            ISecureTokenGenerator secureTokenGenerator,
            IEmailSender emailSender,
            IClock clock,
            IUnitOfWork unitOfWork
        )
        {
            _userRepository = userRepository;
            _emailVerificationTokenRepository = emailVerificationTokenRepository;
            _passwordHasher = passwordHasher;
            _secureTokenGenerator = secureTokenGenerator;
            _emailSender = emailSender;
            _clock = clock;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            // 1. Normalize value objects
            var email = Email.Create(request.Email);
            var username = Username.Create(request.Username);

            // 2. Check uniqueness
            var emailExists = await _userRepository.EmailExistsAsync(email, cancellationToken);
            if (emailExists)
                throw new EmailAlreadyExistsException(request.Email);

            var usernameExists = await _userRepository.UsernameExistsAsync(username, cancellationToken);
            if (usernameExists)
                throw new UsernameAlreadyExistsException(request.Username);

            // 3. Hash password
            var hashed = _passwordHasher.Hash(request.Password);
            var passwordHash = PasswordHash.FromHashed(hashed);

            // 4. Create user aggregate
            var user = User.Register(email, username, passwordHash);

            // 5. Create email verification token
            var token = _secureTokenGenerator.Generate();
            var expiresAt = _clock.UtcNow.AddHours(24);

            var verificationToken = EmailVerificationToken.Create(user.Id, token, expiresAt);

            // 6. Persist
            await _userRepository.AddAsync(user, cancellationToken);

            await _emailVerificationTokenRepository.AddAsync(verificationToken, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 7. Send verification email
            await _emailSender.SendAsync(user.Email.Value, "Verify your email", $"Your verification token: {token}", cancellationToken);

            return user.Id.Value;
        }
    }
}