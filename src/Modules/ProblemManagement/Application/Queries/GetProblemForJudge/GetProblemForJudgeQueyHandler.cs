using MediatR;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemForJudge;
using VAlgo.Modules.ProblemManagement.Domain.Exceptions;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.GetProblemForJudge
{
    public sealed class GetProblemForJudgeQueryHandler : IRequestHandler<GetProblemForJudgeQuery, ProblemForJudgeDto>
    {
        private readonly IProblemForJudgeQueries _problemForJudgeQueries;

        public GetProblemForJudgeQueryHandler(IProblemForJudgeQueries problemForJudgeQueries)
            => _problemForJudgeQueries = problemForJudgeQueries;

        public async Task<ProblemForJudgeDto> Handle(GetProblemForJudgeQuery request, CancellationToken cancellationToken)
        {
            var problem = await _problemForJudgeQueries.GetAsync(request.ProblemId, cancellationToken);

            if (problem == null)
                throw new ProblemNotFoundException(request.ProblemId);

            return problem;
        }
    }
}