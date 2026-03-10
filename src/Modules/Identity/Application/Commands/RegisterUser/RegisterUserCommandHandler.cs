using System.Diagnostics;
using MediatR;
using VAlgo.Modules.Identity.Application.Abstractions.Communication;
using VAlgo.Modules.Identity.Application.Abstractions.Persistence;
using VAlgo.Modules.Identity.Application.Abstractions.Security;
using VAlgo.Modules.Identity.Application.Exceptions;
using VAlgo.Modules.Identity.Application.Persistence;
using VAlgo.Modules.Identity.Domain.Aggregates;
using VAlgo.Modules.Identity.Domain.Exceptions;
using VAlgo.Modules.Identity.Domain.ValueObjects;
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

            var sw = Stopwatch.StartNew();

            var exists = await _userRepository.CheckExistsAsync(email, username, cancellationToken);

            // 2. Check uniqueness
            Console.WriteLine($"Check email: {sw.ElapsedMilliseconds}");
            if (exists.EmailExists)
                throw new EmailAlreadyExistsException(request.Email);

            Console.WriteLine($"Check username: {sw.ElapsedMilliseconds}");
            if (exists.UsernameExists)
                throw new UsernameAlreadyExistsException(request.Username);

            // 3. Hash password
            var hashed = _passwordHasher.Hash(request.Password);
            Console.WriteLine($"Hash password: {sw.ElapsedMilliseconds}");
            var passwordHash = PasswordHash.FromHashed(hashed);

            // 4. Create user aggregate
            var user = User.Register(email, username, passwordHash);

            // 5. Create email verification token
            var token = _secureTokenGenerator.Generate();
            var expiresAt = _clock.UtcNow.AddHours(24);

            var verificationToken = EmailVerificationToken.Create(user.Id, token, expiresAt);

            // 6. Persist
            await _userRepository.AddAsync(user, cancellationToken);
            Console.WriteLine($"Add user: {sw.ElapsedMilliseconds}");

            await _emailVerificationTokenRepository.AddAsync(verificationToken, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            Console.WriteLine($"Save changes: {sw.ElapsedMilliseconds}");

            // 7. Send verification email
            _ = Task.Run(() =>
                _emailSender.SendAsync(user.Email.Value, "Verify your email", $"Your verification token: {token}", CancellationToken.None)
            );

            return user.Id.Value;
        }
    }
}