using MediatR;
using VAlgo.Modules.Identity.Application.Abstractions.Persistence;
using VAlgo.Modules.Identity.Application.Abstractions.Security;
using VAlgo.Modules.Identity.Application.Exceptions;
using VAlgo.Modules.Identity.Application.Persistence;
using VAlgo.Modules.Identity.Domain.Exceptions;
using VAlgo.SharedKernel.Time;

namespace VAlgo.Modules.Identity.Application.Commands.ResetPasswordUser
{
    public sealed class ResetPasswordUserCommandHandler : IRequestHandler<ResetPasswordUserCommand, Unit>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordResetTokenRepository _passwordResetTokenRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IClock _clock;
        private readonly IUnitOfWork _unitOfWork;

        public ResetPasswordUserCommandHandler(
            IUserRepository userRepository,
            IPasswordResetTokenRepository passwordResetTokenRepository,
            IPasswordHasher passwordHasher,
            IClock clock,
            IUnitOfWork unitOfWork
        )
        {
            _userRepository = userRepository;
            _passwordResetTokenRepository = passwordResetTokenRepository;
            _passwordHasher = passwordHasher;
            _clock = clock;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(ResetPasswordUserCommand request, CancellationToken cancellationToken)
        {
            var token = await _passwordResetTokenRepository.GetByTokenAsync(request.Token, cancellationToken);
            if (token == null || token.IsUsed || token.IsExpired(_clock))
                throw new InvalidPasswordResetTokenException();

            var user = await _userRepository.GetByIdAsync(token.UserId, cancellationToken);
            if (user == null)
                throw new UserNotFoundException(token.UserId.Value);

            var hashPassword = _passwordHasher.Hash(request.NewPassword);
            var newPassword = PasswordHash.FromHashed(hashPassword);
            user.ChangePassword(newPassword);

            token.MarkAsUsed();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}