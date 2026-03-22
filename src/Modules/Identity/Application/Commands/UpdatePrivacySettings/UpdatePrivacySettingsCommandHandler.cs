using MediatR;
using VAlgo.Modules.Identity.Application.Abstractions.Persistence;
using VAlgo.Modules.Identity.Application.Persistence;
using VAlgo.Modules.Identity.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Identity.Application.Commands.UpdatePrivacySettings
{
    public sealed class UpdatePrivacySettingsCommandHandler : IRequestHandler<UpdatePrivacySettingsCommand, Unit>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePrivacySettingsCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _userRepository = userRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdatePrivacySettingsCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(UserId.From(_currentUserService.UserId), cancellationToken);

            if (user == null)
                throw new InvalidOperationException("User not found.");

            user.UpdatePrivacySettings(request.ShowRecentSubmissions, request.ShowSubmissionHeatmap);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}