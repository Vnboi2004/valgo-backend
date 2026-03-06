using MediatR;
using VAlgo.Modules.Contests.Application.Interfaces;
using VAlgo.Modules.Contests.Domain.ValueObjects;

namespace VAlgo.Modules.Contests.Application.Queries.GetContestLeaderboard
{
    public sealed class GetContestLeaderboardQueryHandler : IRequestHandler<GetContestLeaderboardQuery, IReadOnlyList<ContestLeaderboardItemDto>>
    {
        private readonly IContestRepository _contestRepository;

        public GetContestLeaderboardQueryHandler(IContestRepository contestRepository)
            => _contestRepository = contestRepository;

        public async Task<IReadOnlyList<ContestLeaderboardItemDto>> Handle(GetContestLeaderboardQuery request, CancellationToken cancellationToken)
        {
            var contestId = ContestId.From(request.ContestId);

            var contest = await _contestRepository.GetByIdAsync(contestId, cancellationToken);
            if (contest == null)
                throw new InvalidOperationException("Contest not found.");

            var ordered = contest.Participants
                .OrderByDescending(x => x.Score)
                .ThenBy(x => x.Penalty)
                .ToList();

            var leaderboard = new List<ContestLeaderboardItemDto>();

            int rank = 1;

            foreach (var participant in ordered)
            {
                leaderboard.Add(new ContestLeaderboardItemDto
                {
                    Rank = rank++,
                    UserId = participant.UserId,
                    Score = participant.Score,
                    Penalty = participant.Penalty,
                    SolvedProblems = participant.Score // tạm thời (nhiều các tính khác nhau) 
                });
            }

            return leaderboard;
        }
    }
}