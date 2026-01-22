using MediatR;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemForJudge;
using VAlgo.Modules.ProblemManagement.Domain.Exceptions;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.GetProblemForJudge
{
    public sealed class GetProblemForJudgeQueryHandler : IRequestHandler<GetProblemForJudgeQuery, ProblemForJudgeDto>
    {
        private readonly IProblemForJudgeReadStore _problemForJudgeReadStore;

        public GetProblemForJudgeQueryHandler(IProblemForJudgeReadStore problemForJudgeReadStore)
            => _problemForJudgeReadStore = problemForJudgeReadStore;

        public async Task<ProblemForJudgeDto> Handle(GetProblemForJudgeQuery request, CancellationToken cancellationToken)
        {
            var problem = await _problemForJudgeReadStore.GetAsync(request.ProblemId, cancellationToken);

            if (problem == null)
                throw new ProblemNotFoundException(request.ProblemId);

            return problem;
        }
    }
}