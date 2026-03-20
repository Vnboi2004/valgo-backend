using MediatR;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemHints
{
    public sealed class GetProblemHintsQueryHandler : IRequestHandler<GetProblemHintsQuery, IReadOnlyList<ProblemHintDto>>
    {
        private readonly IProblemManagementQueries _problemManagementQueries;


        public GetProblemHintsQueryHandler(IProblemManagementQueries problemManagementQueries)
        {
            _problemManagementQueries = problemManagementQueries;
        }

        public async Task<IReadOnlyList<ProblemHintDto>> Handle(GetProblemHintsQuery request, CancellationToken cancellationToken)
        {
            var exists = await _problemManagementQueries.ProblemExistsAsync(request.ProblemId, cancellationToken);

            if (!exists)
                throw new InvalidOperationException("Problem not found");

            return await _problemManagementQueries.GetHintsAsync(request.ProblemId, cancellationToken);
        }
    }
}