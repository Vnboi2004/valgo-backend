using MediatR;
using VAlgo.Modules.Contests.Application.Interfaces;
using VAlgo.Modules.Contests.Application.Leaderboard;
using VAlgo.Modules.Contests.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Contests.Application.Commands.JoinContest
{
    public sealed class JoinContestCommandHandler : IRequestHandler<JoinContestCommand, Unit>
    {
        private readonly IContestRepository _contestRepository;
        private readonly ILeaderboardService _leaderboard;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public JoinContestCommandHandler(
            IContestRepository contestRepository,
            ICurrentUserService currentUserService,
            ILeaderboardService leaderboard,
            IUnitOfWork unitOfWork
        )
        {
            _contestRepository = contestRepository;
            _currentUserService = currentUserService;
            _leaderboard = leaderboard;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(JoinContestCommand request, CancellationToken cancellationToken)
        {
            var contestId = ContestId.From(request.ContestId);
            var userId = _currentUserService.UserId;

            var contest = await _contestRepository.GetByIdAsync(contestId, cancellationToken);
            if (contest == null)
                throw new InvalidOperationException("Contest not found.");

            contest.Join(userId);

            await _contestRepository.UpdateAsync(contest, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _leaderboard.UpdateParticipantAsync(request.ContestId, userId, score: 0, penalty: 0, cancellationToken);

            return Unit.Value;
        }
    }
}