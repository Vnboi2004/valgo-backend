using MediatR;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;
using VAlgo.Modules.ProblemManagement.Domain.Exceptions;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemEditor
{
    public sealed class GetProblemEditorQueryHandler : IRequestHandler<GetProblemEditorQuery, ProblemEditorDto>
    {
        private readonly IProblemReadStore _problemReadStore;

        public GetProblemEditorQueryHandler(IProblemReadStore problemReadStore)
            => _problemReadStore = problemReadStore;

        public async Task<ProblemEditorDto> Handle(GetProblemEditorQuery request, CancellationToken cancellationToken)
        {
            var problem = await _problemReadStore.GetAsync(request.ProblemId, cancellationToken);

            if (problem == null)
                throw new ProblemNotFoundException(request.ProblemId);

            return problem;
        }
    }
}