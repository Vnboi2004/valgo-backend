using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemDetail
{
    public sealed record GetProblemDetailQuery(Guid ProblemId) : IQuery<ProblemDetailDto>;
}