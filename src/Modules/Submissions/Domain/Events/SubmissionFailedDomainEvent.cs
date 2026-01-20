using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Submissions.Domain.Events
{
    public sealed record SubmissionFailedDomainEvent(
        Guid SubmissionId,
        string Reason,
        DateTime OccurredOn
    ) : IDomainEvent;
}