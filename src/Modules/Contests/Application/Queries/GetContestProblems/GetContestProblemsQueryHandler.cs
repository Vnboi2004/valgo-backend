using MediatR;
using VAlgo.Modules.Contests.Application.Interfaces;
using VAlgo.Modules.Contests.Application.Queries.GetContestDetail;
using VAlgo.Modules.Contests.Domain.ValueObjects;

namespace VAlgo.Modules.Contests.Application.Queries.GetContestProblems
{
    public sealed class GetContestProblemsQueryHandler : IRequestHandler<GetContestProblemsQuery, IReadOnlyList<ContestProblemDto>>
    {
        private readonly IContestRepository _contestRepository;

        public GetContestProblemsQueryHandler(IContestRepository contestRepository)
            => _contestRepository = contestRepository;

        public async Task<IReadOnlyList<ContestProblemDto>> Handle(GetContestProblemsQuery request, CancellationToken cancellationToken)
        {
            var contestId = ContestId.From(request.ContestId);

            var contest = await _contestRepository.GetByIdAsync(contestId, cancellationToken);
            if (contest == null)
                throw new InvalidOperationException("Contest not found.");

            return contest.Problems
                .OrderBy(x => x.Order)
                .Select(x => new ContestProblemDto
                {
                    ProblemId = x.ProblemId,
                    Code = x.Code,
                    Order = x.Order,
                    Points = x.Points
                }).ToList();
        }
    }
}