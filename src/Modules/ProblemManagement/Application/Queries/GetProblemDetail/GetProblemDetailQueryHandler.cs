using MediatR;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;
using VAlgo.Modules.ProblemManagement.Domain.Exceptions;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemDetail
{
    public sealed class GetProblemDetailQueryHandler : IRequestHandler<GetProblemDetailQuery, ProblemDetailDto>
    {
        private readonly IProblemManagementQueries _problemManagementQueries;

        public GetProblemDetailQueryHandler(IProblemManagementQueries problemManagementQueries)
            => _problemManagementQueries = problemManagementQueries;

        public async Task<ProblemDetailDto> Handle(GetProblemDetailQuery request, CancellationToken cancellationToken)
        {
            var problem = await _problemManagementQueries.GetDetailAsync(request.ProblemId, cancellationToken);

            if (problem == null)
                throw new ProblemNotFoundException(request.ProblemId);

            return problem;
        }
    }
}