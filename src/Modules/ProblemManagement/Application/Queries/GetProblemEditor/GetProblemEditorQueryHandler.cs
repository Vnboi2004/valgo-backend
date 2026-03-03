using MediatR;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;
using VAlgo.Modules.ProblemManagement.Domain.Exceptions;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemEditor
{
    public sealed class GetProblemEditorQueryHandler : IRequestHandler<GetProblemEditorQuery, ProblemEditorDto>
    {
        private readonly IProblemManagementQueries _problemManagementQueries;

        public GetProblemEditorQueryHandler(IProblemManagementQueries problemManagementQueries)
            => _problemManagementQueries = problemManagementQueries;

        public async Task<ProblemEditorDto> Handle(GetProblemEditorQuery request, CancellationToken cancellationToken)
        {
            var problem = await _problemManagementQueries.GetEditorAsync(request.ProblemId, cancellationToken);

            if (problem == null)
                throw new ProblemNotFoundException(request.ProblemId);

            return problem;
        }
    }
}