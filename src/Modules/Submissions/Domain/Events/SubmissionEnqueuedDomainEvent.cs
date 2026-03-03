using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Submissions.Domain.Events
{
    public sealed record SubmissionEnqueuedDomainEvent(Guid SubmissionId, DateTime OccurredOn)
        : IDomainEvent;
}