using MediatR;
using VAlgo.Modules.Contests.Application.Interfaces;
using VAlgo.Modules.Contests.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Contests.Application.Commands.RegisterContest
{
    public sealed class RegisterContestCommandHandler : IRequestHandler<RegisterContestCommand, Unit>
    {
        private readonly IContestRepository _contestRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterContestCommandHandler(IContestRepository contestRepository, ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
        {
            _contestRepository = contestRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(RegisterContestCommand request, CancellationToken cancellationToken)
        {
            var contest = await _contestRepository.GetByIdAsync(ContestId.From(request.ContestId), cancellationToken);
            if (contest == null)
                throw new InvalidOperationException("Contest not found.");

            contest.Register(_currentUserService.UserId);

            await _contestRepository.UpdateAsync(contest, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}