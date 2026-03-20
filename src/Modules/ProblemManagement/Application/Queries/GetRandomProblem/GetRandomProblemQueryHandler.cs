using MediatR;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetRandomProblem
{
    public sealed class GetRandomProblemQueryHandler : IRequestHandler<GetRandomProblemQuery, RandomProblemDto>
    {
        private readonly IProblemManagementQueries _problemManagementQueries;

        public GetRandomProblemQueryHandler(IProblemManagementQueries problemManagementQueries)
        {
            _problemManagementQueries = problemManagementQueries;
        }

        public async Task<RandomProblemDto> Handle(GetRandomProblemQuery request, CancellationToken cancellationToken)
        {
            var problem = await _problemManagementQueries.GetRandomAsync(request.Difficulty, request.ClassificationId, cancellationToken)
                ?? throw new InvalidOperationException("Problem not found.");

            return problem;
        }
    }
}