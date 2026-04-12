using VAlgo.Modules.Contests.Application.Interfaces;
using VAlgo.Modules.Contests.Application.Leaderboard;
using VAlgo.Modules.Contests.Application.Mappers;
using VAlgo.Modules.Contests.Application.Realtime;
using VAlgo.Modules.Contests.Domain.Enums;
using VAlgo.Modules.Contests.Domain.ValueObjects;
using VAlgo.SharedKernel.Contracts.Events;
using VAlgo.SharedKernel.Infrastructure.Redis;

namespace VAlgo.Modules.Contests.Application.Consumers
{
    public sealed class SubmissionResultConsumer
    {
        private readonly IContestRepository _contestRepository;
        private readonly ILeaderboardService _leaderboardService;
        private readonly RedisDatabaseProvider _redis;
        private readonly IContestLeaderboardNotifier _leaderboardNotifier;
        private readonly IUnitOfWork _unitOfWork;

        public SubmissionResultConsumer(
            IContestRepository contestRepository,
            ILeaderboardService leaderboardService,
            RedisDatabaseProvider redis,
            IContestLeaderboardNotifier leaderboardNotifier,
            IUnitOfWork unitOfWork
        )
        {
            _contestRepository = contestRepository;
            _leaderboardService = leaderboardService;
            _redis = redis;
            _leaderboardNotifier = leaderboardNotifier;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(SubmissionResultIntegrationEvent @event, CancellationToken cancellationToken)
        {
            if (@event.ContestId == null)
                return;

            var contest = await _contestRepository.GetByIdAsync(ContestId.From(@event.ContestId.Value), cancellationToken);
            if (contest == null)
                return;

            var participant = contest.GetParticipant(@event.UserId);
            if (participant == null)
                return;

            var stat = participant.GetStat(@event.ProblemId);
            var wasSolved = stat.IsSolved;

            var contestVerdict = VerdictMapper.ToContestVerdict(@event.Verdict);

            contest.ProcessSubmission(@event.UserId, @event.ProblemId, contestVerdict, @event.SubmittedAt);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var db = _redis.GetDatabase();

            if (!wasSolved && stat.IsSolved)
            {
                await db.HashIncrementAsync($"contest:{@event.ContestId}:solved", @event.UserId.ToString(), 1);
            }

            if (!contest.IsLeaderboardFrozen)
            {
                await _leaderboardService.UpdateParticipantAsync(
                    @event.ContestId.Value,
                    @event.UserId,
                    participant.Score,
                    participant.Penalty
                );
            }


            await _leaderboardNotifier.NotifyLeaderboardUpdated(@event.ContestId.Value);
        }
    }
}