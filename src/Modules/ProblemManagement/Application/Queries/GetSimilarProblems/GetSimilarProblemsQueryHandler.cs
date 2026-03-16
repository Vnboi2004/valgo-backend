using MediatR;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetSimilarProblems
{
    public sealed class GetSimilarProblemsCommandHandler : IRequestHandler<GetSimilarProblemsQuery, IReadOnlyList<SimilarProblemDto>>
    {
        private readonly IProblemManagementQueries _problemManagementQueries;

        public GetSimilarProblemsCommandHandler(IProblemManagementQueries problemManagementQueries)
            => _problemManagementQueries = problemManagementQueries;

        public async Task<IReadOnlyList<SimilarProblemDto>> Handle(GetSimilarProblemsQuery request, CancellationToken cancellationToken)
        {
            return await _problemManagementQueries.GetSimilarProblemsAsync(request.ProblemId, cancellationToken);
        }
    }
}