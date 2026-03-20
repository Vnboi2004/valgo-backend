using VAlgo.SharedKernel.Abstractions;
using VAlgo.SharedKernel.CrossModule.Submissions;

namespace VAlgo.Modules.Submissions.Domain.Events
{
    public sealed record SubmissionFailedDomainEvent(
        Guid SubmissionId,
        SubmissionFailureReason Reason,
        DateTime OccurredOn
    ) : IDomainEvent;
}