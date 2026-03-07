using VAlgo.Modules.Submissions.Domain.Enums;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Submissions.Domain.Events
{
    public sealed record SubmissionCompletedDomainEvent(
        Guid SubmissionId,
        Guid UserId,
        Guid ProblemId,
        Guid? ContestId,
        Verdict Verdict,
        DateTime OccurredOn
    ) : IDomainEvent;
}