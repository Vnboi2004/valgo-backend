using MediatR;
using VAlgo.Modules.Contests.Application.Interfaces;
using VAlgo.Modules.Contests.Domain.Enums;
using VAlgo.Modules.Contests.Domain.ValueObjects;
using VAlgo.SharedKernel.IntegrationEvents;

namespace VAlgo.Modules.Contests.Application.EventHandlers
{
    public sealed class SubmissionCompletedIntegrationEventHandler : INotificationHandler<SubmissionCompletedIntegrationEvent>
    {
        private readonly IContestRepository _contestRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SubmissionCompletedIntegrationEventHandler(IContestRepository contestRepository, IUnitOfWork unitOfWork)
        {
            _contestRepository = contestRepository;
            _unitOfWork = unitOfWork;
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
        }
    }
}