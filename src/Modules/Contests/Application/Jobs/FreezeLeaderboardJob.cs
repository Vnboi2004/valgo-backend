using VAlgo.Modules.Contests.Application.Interfaces;
using VAlgo.Modules.Contests.Application.Realtime;

namespace VAlgo.Modules.Contests.Application.Jobs
{
    public sealed class FreezeLeaderboardJob
    {
        private readonly IContestRepository _contestRepository;
        private readonly IContestLeaderboardNotifier _contestLeaderboardNotifier;
        private readonly IUnitOfWork _unitOfWork;

        public FreezeLeaderboardJob(
            IContestRepository contestRepository,
            IContestLeaderboardNotifier contestLeaderboardNotifier,
            IUnitOfWork unitOfWork
        )
        {
            _contestRepository = contestRepository;
            _contestLeaderboardNotifier = contestLeaderboardNotifier;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;

            var contests = await _contestRepository
                .GetRunningContestsToFreezeAsync(now, cancellationToken);

            var frozenContests = new List<Guid>();

            foreach (var contest in contests)
            {
                if (contest.FreezeLeaderboard(now))
                {
                    frozenContests.Add(contest.Id.Value);
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            foreach (var contestId in frozenContests)
            {
                await _contestLeaderboardNotifier.NotifyLeaderboardFrozen(contestId);
            }
        }
    }
}