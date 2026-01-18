using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Submissions.Application.Queries.GetSubmissionDetail
{
    public sealed record GetSubmissionDetailQuery(Guid SubmissionId) : IQuery<SubmissionDetailDto>;
}