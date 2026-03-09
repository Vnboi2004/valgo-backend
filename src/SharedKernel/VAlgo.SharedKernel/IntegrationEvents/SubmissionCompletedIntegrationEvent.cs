using MediatR;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.SharedKernel.IntegrationEvents
{
    public sealed record SubmissionCompletedIntegrationEvent(
        Guid SubmissionId,
        Guid UserId,
        Guid ProblemId,
        Guid? ContestId,
        int Verdict,
        DateTime FinishedAt
    ) : INotification;
}