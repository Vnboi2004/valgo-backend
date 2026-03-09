using MediatR;
using VAlgo.Modules.Contests.Application.Interfaces;
using VAlgo.Modules.Contests.Domain.ValueObjects;

namespace VAlgo.Modules.Contests.Application.Queries.GetContestDetail
{
    public sealed class GetContestDetailQueryHandler : IRequestHandler<GetContestDetailQuery, ContestDetailDto>
    {
        private readonly IContestRepository _contestRepository;

        public GetContestDetailQueryHandler(IContestRepository contestRepository)
            => _contestRepository = contestRepository;

        public async Task<ContestDetailDto> Handle(GetContestDetailQuery request, CancellationToken cancellationToken)
        {
            var contestId = ContestId.From(request.ContestId);

            var contest = await _contestRepository.GetByIdAsync(contestId, cancellationToken);
            if (contest == null)
                throw new InvalidOperationException("Contest not found.");

            return new ContestDetailDto
            {
                Id = contest.Id.Value,
                Title = contest.Title,
                Description = contest.Description,
                StartTime = contest.StartTime,
                EndTime = contest.EndTime,
                Status = contest.Status,
                Visibility = contest.Visibility,
                MaxParticipants = contest.MaxParticipants,
                CreatedBy = contest.CreatedBy,
                CreatedAt = contest.CreatedAt,
                ParticipantCount = contest.Participants.Count,
                Problems = contest.Problems
                    .OrderBy(x => x.Order)
                    .Select(x => new ContestProblemDto
                    {
                        ProblemId = x.ProblemId,
                        Code = x.Code,
                        Order = x.Order,
                        Points = x.Points
                    }).ToList()
            };
        }
    }
}