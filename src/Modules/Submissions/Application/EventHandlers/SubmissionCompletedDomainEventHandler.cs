using MediatR;
using VAlgo.Modules.Submissions.Domain.Events;
using VAlgo.SharedKernel.IntegrationEvents;

namespace VAlgo.Modules.Submissions.Application.EventHandlers
{
    public sealed class SubmissionCompletedDomainEventHandler : INotificationHandler<SubmissionCompletedDomainEvent>
    {
        private readonly IMediator _mediator;

        public SubmissionCompletedDomainEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(
            SubmissionCompletedDomainEvent notification,
            CancellationToken cancellationToken)
        {
            var integrationEvent = new SubmissionCompletedIntegrationEvent(
                notification.SubmissionId,
                notification.UserId,
                notification.ProblemId,
                notification.ContestId,
                (int)notification.Verdict,
                notification.OccurredOn
            );

            await _mediator.Publish(integrationEvent, cancellationToken);
        }
    }
}