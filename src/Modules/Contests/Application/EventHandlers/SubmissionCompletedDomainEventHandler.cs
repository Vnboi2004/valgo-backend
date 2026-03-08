using FluentValidation.Internal;
using MediatR;
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
        private readonly IContestLeaderboardNotifier _notifier;

        public SubmissionCompletedIntegrationEventHandler(
            IContestRepository contestRepository,
            IUnitOfWork unitOfWork,
            ILeaderboardService leaderboard,
            IContestLeaderboardNotifier notifier
        )
        {
            _contestRepository = contestRepository;
            _unitOfWork = unitOfWork;
            _leaderboard = leaderboard;
            _notifier = notifier;
        }

        public async Task Handle(
            SubmissionCompletedIntegrationEvent notification,
            CancellationToken cancellationToken)
        {
            if (notification.ContestId is null)
                return;

            var contest = await _contestRepository.GetByIdAsync(
                ContestId.From(notification.ContestId.Value),
                cancellationToken);

            if (contest is null)
                return;

            var verdict = (ContestSubmissionVerdict)notification.Verdict;

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

            await _notifier.NotifyLeaderboardUpdated(contest.Id.Value);
        }
    }
}