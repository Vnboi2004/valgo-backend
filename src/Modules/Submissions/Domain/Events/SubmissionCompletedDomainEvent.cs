using VAlgo.SharedKernel.Abstractions;
using VAlgo.SharedKernel.CrossModule.Submissions;

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