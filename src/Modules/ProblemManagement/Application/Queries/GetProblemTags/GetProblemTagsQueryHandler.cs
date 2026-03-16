using MediatR;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemTags
{
    public sealed class GetProblemTagsQueryHandler : IRequestHandler<GetProblemTagsQuery, IReadOnlyList<ProblemTagDto>>
    {
        private readonly IProblemManagementQueries _problemManagementQueries;

        public GetProblemTagsQueryHandler(IProblemManagementQueries problemManagementQueries)
            => _problemManagementQueries = problemManagementQueries;

        public async Task<IReadOnlyList<ProblemTagDto>> Handle(GetProblemTagsQuery request, CancellationToken cancellationToken)
        {
            return await _problemManagementQueries.GetProblemTagsAsync(request.ProblemId, cancellationToken);
        }
    }
}