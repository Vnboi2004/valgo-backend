using MediatR;
using VAlgo.Modules.Contests.Application.Interfaces;
using VAlgo.Modules.Contests.Domain.ValueObjects;

namespace VAlgo.Modules.Contests.Application.Queries.GetContestParticipants
{
    public sealed class GetContestParticipantsQueryHandler : IRequestHandler<GetContestParticipantsQuery, ContestParticipantDto>
    {
        private readonly IContestRepository _contestRepository;

        public GetContestParticipantsQueryHandler(IContestRepository contestRepository)
            => _contestRepository = contestRepository;

        public async Task<ContestParticipantDto> Handle(GetContestParticipantsQuery request, CancellationToken cancellationToken)
        {
            var contestId = ContestId.From(request.ContestId);

            var contest = await _contestRepository.GetByIdAsync(contestId, cancellationToken);

            if (contest == null)
                throw new InvalidOperationException("Contest not found.");

            var participants = contest.Participants
                .OrderByDescending(x => x.Score)
                .ThenBy(x => x.Penalty)
                .Select(x => new ContestParticipantItemDto
                {
                    UserId = x.UserId,
                    JoinedAt = x.JoinedAt
                })
                .ToList();


            return new ContestParticipantDto
            {
                TotalCount = participants.Count,
                Participants = participants
            };
        }
    }
}