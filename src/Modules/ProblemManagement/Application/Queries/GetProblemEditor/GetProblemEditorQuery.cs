using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemEditor
{
    public sealed record GetProblemEditorQuery(Guid ProblemId) : IQuery<ProblemEditorDto>;
}