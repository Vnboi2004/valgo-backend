using MediatR;
using VAlgo.Modules.Contests.Application.Interfaces;
using VAlgo.Modules.Contests.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Contests.Application.Commands.JoinContest
{
    public sealed class JoinContestCommandHandler : IRequestHandler<JoinContestCommand, Unit>
    {
        private readonly IContestRepository _contestRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public JoinContestCommandHandler(IContestRepository contestRepository, ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
        {
            _contestRepository = contestRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(JoinContestCommand request, CancellationToken cancellationToken)
        {
            var contestId = ContestId.From(request.ContestId);

            var contest = await _contestRepository.GetByIdAsync(contestId, cancellationToken);
            if (contest == null)
                throw new InvalidOperationException("Contest not found.");

            contest.Join(_currentUserService.UserId);

            await _contestRepository.UpdateAsync(contest, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}