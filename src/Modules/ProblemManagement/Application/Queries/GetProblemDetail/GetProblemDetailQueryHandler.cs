using MediatR;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;
using VAlgo.Modules.ProblemManagement.Domain.Exceptions;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemDetail
{
    public sealed class GetProblemDetailQueryHandler : IRequestHandler<GetProblemDetailQuery, ProblemDetailDto>
    {
        private readonly IProblemReadStore _problemReadStore;

        public GetProblemDetailQueryHandler(IProblemReadStore problemReadStore)
            => _problemReadStore = problemReadStore;

        public async Task<ProblemDetailDto> Handle(GetProblemDetailQuery request, CancellationToken cancellationToken)
        {
            var problem = await _problemReadStore.GetProblemDetailAsync(request.ProblemId, cancellationToken);

            if (problem == null)
                throw new ProblemNotFoundException(request.ProblemId);

            return problem;
        }
    }
}