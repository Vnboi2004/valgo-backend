using FluentValidation.Internal;
using MediatR;
using Microsoft.Extensions.Logging;
using VAlgo.Modules.Contests.Application.Interfaces;
using VAlgo.Modules.Contests.Application.Leaderboard;
using VAlgo.Modules.Contests.Application.Realtime;
using VAlgo.Modules.Contests.Domain.Enums;
using VAlgo.Modules.Contests.Domain.ValueObjects;
using VAlgo.SharedKernel.IntegrationEvents;

namespace VAlgo.Modules.Contests.Application.EventHandlers
{
    public sealed class SubmissionCompletedIntegrationEventHandler : INotificationHandler<SubmissionCompletedIntegrationEvent>
    {
        private readonly IContestRepository _contestRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILeaderboardService _leaderboard;
        private readonly ILeaderboardCacheService _cache;
        private readonly ILogger<SubmissionCompletedIntegrationEventHandler> _logger;
        private readonly IContestLeaderboardNotifier _notifier;

        public SubmissionCompletedIntegrationEventHandler(
            IContestRepository contestRepository,
            IUnitOfWork unitOfWork,
            ILeaderboardService leaderboard,
            ILeaderboardCacheService cache,
            IContestLeaderboardNotifier notifier,
            ILogger<SubmissionCompletedIntegrationEventHandler> logger
        )
        {
            _contestRepository = contestRepository;
            _unitOfWork = unitOfWork;
            _leaderboard = leaderboard;
            _cache = cache;
            _notifier = notifier;
            _logger = logger;
        }

        public async Task Handle(
            SubmissionCompletedIntegrationEvent notification,
            CancellationToken cancellationToken)
        {
            Console.WriteLine("🔥 IntegrationEventHandler HIT");
            if (notification.ContestId is null)
                return;

            var contest = await _contestRepository.GetByIdAsync(ContestId.From(notification.ContestId.Value), cancellationToken);

            if (contest is null)
                return;

            var verdict = (ContestSubmissionVerdict)notification.Verdict;

            try
            {
                contest.ProcessSubmission(
                    notification.UserId,
                    notification.ProblemId,
                    verdict,
                    notification.FinishedAt
                );

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var participant = contest.GetParticipant(notification.UserId);

                if (participant == null)
                    return;

                await _leaderboard.UpdateParticipantAsync(contest.Id.Value, participant.UserId, participant.Score, participant.Penalty);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR IN CONTEST HANDLER");
            }


            await _cache.InvalidateAsync(contest.Id.Value);

            await _notifier.NotifyLeaderboardUpdated(contest.Id.Value);
        }
    }
}