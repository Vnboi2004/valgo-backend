using MediatR;
using VAlgo.Modules.Contests.Application.Interfaces;
using VAlgo.Modules.Contests.Application.Leaderboard;
using VAlgo.Modules.Contests.Application.Mappers;
using VAlgo.Modules.Contests.Domain.ValueObjects;
using VAlgo.SharedKernel.Infrastructure.Redis;

namespace VAlgo.Modules.Contests.Application.Commands.RejudgeContest
{
    public sealed class RejudgeContestCommandHandler : IRequestHandler<RejudgeContestCommand, Unit>
    {
        private readonly IContestRepository _contestRepository;
        private readonly ISubmissionReadService _submissionReadService;
        private readonly ILeaderboardService _leaderboardService;
        private readonly RedisDatabaseProvider _redis;
        private readonly IUnitOfWork _unitOfWork;

        public RejudgeContestCommandHandler(
            IContestRepository contestRepository,
            ISubmissionReadService submissionReadService,
            ILeaderboardService leaderboardService,
            RedisDatabaseProvider redis,
            IUnitOfWork unitOfWork
        )
        {
            _contestRepository = contestRepository;
            _submissionReadService = submissionReadService;
            _leaderboardService = leaderboardService;
            _redis = redis;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(RejudgeContestCommand request, CancellationToken cancellationToken)
        {
            var contest = await _contestRepository.GetByIdAsync(ContestId.From(request.ContestId), cancellationToken);
            if (contest == null)
                throw new InvalidOperationException("Contest not found.");

            // 1. Reset domain state
            contest.ResetLeaderboard();

            // 2. Xoá Redis leaderboard
            var db = _redis.GetDatabase();
            await db.KeyDeleteAsync($"contest:{request.ContestId}:leaderboard");
            await db.KeyDeleteAsync($"contest:{request.ContestId}:solved");

            // 3. Lấy submissions
            var submissions = await _submissionReadService.GetByContestIdAsync(request.ContestId, cancellationToken);

            // 4. Sort theo thời gian
            var ordered = submissions
                .OrderBy(x => x.SubmittedAt)
                .ToList();

            // 5. Replay
            foreach (var submission in ordered)
            {
                var contestVerdict = VerdictMapper.ToContestVerdict(submission.Verdict);

                contest.ProcessSubmission(
                    submission.UserId,
                    submission.ProblemId,
                    contestVerdict,
                    submission.SubmittedAt
                );
            }

            // 6. Build lại Redis leaderboard
            foreach (var participant in contest.Participants)
            {
                await _leaderboardService.UpdateParticipantAsync(
                    request.ContestId,
                    participant.UserId,
                    participant.Score,
                    participant.Penalty
                );
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}