using MediatR;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemTags
{
    public sealed record GetProblemTagsQuery(Guid ProblemId) : IRequest<IReadOnlyList<ProblemTagDto>>;
}