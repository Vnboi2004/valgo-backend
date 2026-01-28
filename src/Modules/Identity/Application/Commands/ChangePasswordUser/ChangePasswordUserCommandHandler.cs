using MediatR;
using VAlgo.Modules.Identity.Application.Abstractions;
using VAlgo.Modules.Identity.Application.Abstractions.Persistence;
using VAlgo.Modules.Identity.Application.Abstractions.Security;
using VAlgo.Modules.Identity.Application.Exceptions;
using VAlgo.Modules.Identity.Domain.Exceptions;
using VAlgo.Modules.Identity.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Identity.Application.Commands.ChangePasswordUser
{
    public sealed class ChangePasswordUserCommandHandler : IRequestHandler<ChangePasswordUserCommand, Unit>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ChangePasswordUserCommandHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork
        )
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(ChangePasswordUserCommand request, CancellationToken cancellationToken)
        {
            var userId = UserId.From(request.UserId);

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
                throw new UserNotFoundException(request.UserId);

            var verifyPassword = _passwordHasher.Verify(request.CurrentPassword, user.PasswordHash.Value);
            if (!verifyPassword)
                throw new InvalidCurrentPasswordException();

            var passwordHash = _passwordHasher.Hash(request.NewPassword);
            var newPassword = PasswordHash.FromHashed(passwordHash);

            user.ChangePassword(newPassword);

            await _refreshTokenRepository.RevokeAllAsync(user.Id, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}