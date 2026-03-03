using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemForJudge
{
    public sealed record GetProblemForJudgeQuery(Guid ProblemId) : IQuery<ProblemForJudgeDto>;
}