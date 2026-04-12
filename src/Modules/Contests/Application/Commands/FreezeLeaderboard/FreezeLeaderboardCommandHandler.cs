using MediatR;
using VAlgo.Modules.Contests.Application.Interfaces;
using VAlgo.Modules.Contests.Application.Leaderboard;
using VAlgo.Modules.Contests.Application.Queries.GetContestLeaderboard;
using VAlgo.Modules.Contests.Domain.ValueObjects;

namespace VAlgo.Modules.Contests.Application.Commands.FreezeLeaderboard
{
    public sealed class FreezeLeaderboardCommandHandler : IRequestHandler<FreezeLeaderboardCommand, Unit>
    {
        private readonly IContestRepository _contestRepository;
        private readonly ILeaderboardService _leaderboardService;
        private readonly ILeaderboardSnapshotService _snapshotService;
        private readonly IUnitOfWork _unitOfWork;

        public FreezeLeaderboardCommandHandler(
            IContestRepository contestRepository,
            ILeaderboardService leaderboardService,
            ILeaderboardSnapshotService snapshotService,
            IUnitOfWork unitOfWork)
        {
            _contestRepository = contestRepository;
            _leaderboardService = leaderboardService;
            _snapshotService = snapshotService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(FreezeLeaderboardCommand request, CancellationToken cancellationToken)
        {
            var contestId = ContestId.From(request.ContestId);

            var contest = await _contestRepository.GetByIdAsync(contestId, cancellationToken);

            if (contest == null)
                throw new InvalidOperationException("Contest not found.");

            // 1. Lấy leaderboard hiện tại
            var data = await _leaderboardService.GetTopAsync(request.ContestId, 100);

            var snapshot = new List<ContestLeaderboardItemDto>();

            int rank = 1;

            foreach (var item in data)
            {
                snapshot.Add(new ContestLeaderboardItemDto
                {
                    Rank = rank++,
                    UserId = item.UserId,
                    Score = item.Score,
                    Penalty = item.Penalty,
                    SolvedProblems = item.SolvedProblems
                });
            }

            // 2. Save snapshot
            await _snapshotService.SaveSnapshotAsync(request.ContestId, snapshot);

            // 3. Freeze domain
            contest.FreezeLeaderboard(DateTime.UtcNow);

            await _contestRepository.UpdateAsync(contest, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}