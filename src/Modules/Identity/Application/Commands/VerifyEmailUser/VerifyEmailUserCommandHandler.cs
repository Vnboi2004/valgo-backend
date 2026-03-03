using MediatR;
using VAlgo.Modules.Identity.Application.Abstractions.Persistence;
using VAlgo.Modules.Identity.Application.Exceptions;
using VAlgo.Modules.Identity.Application.Persistence;
using VAlgo.SharedKernel.Time;

namespace VAlgo.Modules.Identity.Application.Commands.VerifyEmailUser
{
    public sealed class VerifyEmailUserCommandHandler : IRequestHandler<VerifyEmailUserCommand, Unit>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailVerificationTokenRepository _emailVerificationTokenRepository;
        private readonly IClock _clock;
        private readonly IUnitOfWork _unitOfWork;

        public VerifyEmailUserCommandHandler(
            IUserRepository userRepository,
            IEmailVerificationTokenRepository emailVerificationTokenRepository,
            IClock clock,
            IUnitOfWork unitOfWork
        )
        {
            _userRepository = userRepository;
            _emailVerificationTokenRepository = emailVerificationTokenRepository;
            _clock = clock;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(VerifyEmailUserCommand request, CancellationToken cancellationToken)
        {
            var token = await _emailVerificationTokenRepository.GetByTokenAsync(request.Token, cancellationToken);
            if (token == null)
                throw new InvalidEmailVerificationTokenException();

            if (token.IsUsed || token.IsExpired(_clock))
                throw new InvalidEmailVerificationTokenException();

            var user = await _userRepository.GetByIdAsync(token.UserId, cancellationToken);
            if (user == null)
                throw new UserNotFoundException(token.UserId.Value);

            user.VerifyEmail();

            token.MarkAsUsed();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}