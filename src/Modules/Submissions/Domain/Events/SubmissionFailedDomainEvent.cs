using VAlgo.Modules.Submissions.Domain.Enums;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Submissions.Domain.Events
{
    public sealed record SubmissionFailedDomainEvent(
        Guid SubmissionId,
        SubmissionFailureReason Reason,
        DateTime OccurredOn
    ) : IDomainEvent;
}