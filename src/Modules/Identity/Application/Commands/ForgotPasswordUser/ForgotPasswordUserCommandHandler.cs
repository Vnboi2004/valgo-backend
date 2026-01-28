using MediatR;
using VAlgo.Modules.Identity.Application.Abstractions;
using VAlgo.Modules.Identity.Application.Abstractions.Communication;
using VAlgo.Modules.Identity.Application.Abstractions.Persistence;
using VAlgo.Modules.Identity.Application.Abstractions.Security;
using VAlgo.Modules.Identity.Domain.Aggregates;
using VAlgo.Modules.Identity.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;
using VAlgo.SharedKernel.Time;

namespace VAlgo.Modules.Identity.Application.Commands.ForgotPasswordUser
{
    public sealed class ForgotPasswordUserCommandHandler : IRequestHandler<ForgotPasswordUserCommand, Unit>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordResetTokenRepository _passwordResetTokenRepository;
        private readonly IEmailSender _emailSender;
        private readonly ISecureTokenGenerator _secureTokenGenerator;
        private readonly IClock _clock;
        private readonly IUnitOfWork _unitOfWork;

        public ForgotPasswordUserCommandHandler(
            IUserRepository userRepository,
            IPasswordResetTokenRepository passwordResetTokenRepository,
            IPasswordHasher passwordHasher,
            IEmailSender emailSender,
            ISecureTokenGenerator secureTokenGenerator,
            IClock clock,
            IUnitOfWork unitOfWork
        )
        {
            _userRepository = userRepository;
            _passwordResetTokenRepository = passwordResetTokenRepository;
            _emailSender = emailSender;
            _secureTokenGenerator = secureTokenGenerator;
            _clock = clock;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(ForgotPasswordUserCommand request, CancellationToken cancellationToken)
        {
            var email = Email.Create(request.Email);

            var user = await _userRepository.GetByEmailAsync(email, cancellationToken);
            if (user == null)
                return Unit.Value;

            var tokenValue = _secureTokenGenerator.Generate();
            var expiresAt = _clock.UtcNow.AddMinutes(30);

            var resetToken = PasswordResetToken.Create(user.Id, tokenValue, expiresAt);

            await _passwordResetTokenRepository.AddAsync(resetToken, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _emailSender.SendAsync(user.Email.Value, "Reset your password", $"Your password reset token:{tokenValue}", cancellationToken);

            return Unit.Value;
        }
    }
}