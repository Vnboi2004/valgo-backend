using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Submissions.Domain.Events
{
    public sealed record SubmissionCreatedDomainEvent(Guid SubmissionId, DateTime OccurredOn) : IDomainEvent;
}