using VAlgo.Modules.Submissions.Domain.Enums;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Submissions.Domain.Events
{
    public sealed record SubmissionCompletedDomainEvent(
        Guid SubmissionId,
        Verdict Verdict,
        DateTime OccurredOn
    ) : IDomainEvent;
}