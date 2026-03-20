using MediatR;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetClassificationStats
{
    public sealed class GetClassificationStatsQueryHandler : IRequestHandler<GetClassificationStatsQuery, IReadOnlyList<ClassificationStatsDto>>
    {
        private readonly IProblemManagementQueries _problemManagementQueries;

        public GetClassificationStatsQueryHandler(IProblemManagementQueries problemManagementQueries)
        {
            _problemManagementQueries = problemManagementQueries;
        }

        public async Task<IReadOnlyList<ClassificationStatsDto>> Handle(GetClassificationStatsQuery request, CancellationToken cancellationToken)
        {
            return await _problemManagementQueries.GetClassificationStatsAsync(request.Type, cancellationToken);
        }
    }
}