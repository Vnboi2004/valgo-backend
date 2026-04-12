using MediatR;
using VAlgo.Modules.Contests.Application.Interfaces;
using VAlgo.Modules.Contests.Application.Leaderboard;
using VAlgo.Modules.Contests.Application.Mappers;
using VAlgo.Modules.Contests.Domain.Enums;
using VAlgo.Modules.Contests.Domain.ValueObjects;
using VAlgo.SharedKernel.Infrastructure.Redis;

namespace VAlgo.Modules.Contests.Application.Commands.RemoveParticipant
{
    public sealed class RemoveParticipantCommandHandler : IRequestHandler<RemoveParticipantCommand, Unit>
    {
        private readonly IContestRepository _contestRepository;
        private readonly ISubmissionReadService _submissionReadService;
        private readonly ILeaderboardService _leaderboardService;
        private readonly RedisDatabaseProvider _redis;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveParticipantCommandHandler(
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

        public async Task<Unit> Handle(RemoveParticipantCommand request, CancellationToken cancellationToken)
        {
            var contest = await _contestRepository.GetByIdAsync(ContestId.From(request.ContestId), cancellationToken);
            if (contest == null)
                throw new InvalidOperationException("Contest not found.");

            // không cho remove khi đang running (optional rule)
            if (contest.Status == ContestStatus.Running)
                throw new InvalidOperationException("Cannot remove participant during running contest.");

            // 1. Remove khỏi domain
            contest.RemoveParticipant(request.UserId);

            // 2. Reset state
            contest.ResetLeaderboard();

            // 3. Xoá Redis
            var db = _redis.GetDatabase();

            await db.KeyDeleteAsync($"contest:{request.ContestId}:leaderboard");
            await db.KeyDeleteAsync($"contest:{request.ContestId}:solved");

            // 4. Lấy submissions (trừ user bị remove)
            var submissions = await _submissionReadService
                .GetByContestIdAsync(request.ContestId, cancellationToken);

            var filtered = submissions
                .Where(x => x.UserId != request.UserId)
                .OrderBy(x => x.SubmittedAt)
                .ToList();

            // 5. Replay
            foreach (var submission in filtered)
            {
                var contestVerdict = VerdictMapper.ToContestVerdict(submission.Verdict);

                contest.ProcessSubmission(
                    submission.UserId,
                    submission.ProblemId,
                    contestVerdict,
                    submission.SubmittedAt
                );
            }

            // 6. Rebuild Redis leaderboard
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