using VAlgo.Modules.Contests.Application.Interfaces;
using VAlgo.Modules.Contests.Application.Realtime;

namespace VAlgo.Modules.Contests.Application.Jobs
{
    public sealed class AutoStartContestJob
    {
        private readonly IContestRepository _contestRepository;
        private readonly IContestLeaderboardNotifier _notifier;
        private readonly IUnitOfWork _unitOfWork;

        public AutoStartContestJob(IContestRepository contestRepository, IContestLeaderboardNotifier notifier, IUnitOfWork unitOfWork)
        {
            _contestRepository = contestRepository;
            _notifier = notifier;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;

            var contests = await _contestRepository.GetPublishedContestsToStartAsync(now, cancellationToken);

            var startedContests = new List<Guid>();

            foreach (var contest in contests)
            {
                if (contest.AutoStart(now))
                {
                    startedContests.Add(contest.Id.Value);
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            foreach (var contestId in startedContests)
            {
                await _notifier.NotifyContestStarted(contestId);
            }
        }
    }
}