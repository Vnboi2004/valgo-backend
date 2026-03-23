using MediatR;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetCodeTemplateForJudge
{
    public sealed record GetCodeTemplateForJudgeQuery(Guid ProblemId, string Language) : IRequest<CodeTemplateForJudgeDto>;
}