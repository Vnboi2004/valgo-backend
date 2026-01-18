using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Submissions.Application.Queries.GetSubmissionStatus
{
    public sealed record GetSubmissionStatusQuery(Guid SubmissionId) : IQuery<SubmissionStatusDto>;
}