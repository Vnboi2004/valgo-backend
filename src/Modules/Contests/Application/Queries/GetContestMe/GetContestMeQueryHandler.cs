using MediatR;
using VAlgo.Modules.Contests.Application.Interfaces;
using VAlgo.Modules.Contests.Domain.Enums;
using VAlgo.Modules.Contests.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Contests.Application.Queries.GetContestMe
{
    public sealed class GetContestMeQueryHandler : IRequestHandler<GetContestMeQuery, ContestMeDto>
    {
        private readonly IContestRepository _contestRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetContestMeQueryHandler(IContestRepository contestRepository, ICurrentUserService currentUserService)
        {
            _contestRepository = contestRepository;
            _currentUserService = currentUserService;
        }

        public async Task<ContestMeDto> Handle(GetContestMeQuery request, CancellationToken cancellationToken)
        {
            var contest = await _contestRepository.GetByIdAsync(ContestId.From(request.ContestId), cancellationToken);
            if (contest == null)
                throw new InvalidOperationException("Contest not found.");

            var userId = _currentUserService.UserId;

            var participant = contest.Participants.FirstOrDefault(x => x.UserId == userId);
            var isRegistered = participant?.IsRegistered ?? false;
            var hasJoined = participant?.HasJoined ?? false;

            return new ContestMeDto
            {
                IsRegistered = isRegistered,
                HasJoined = hasJoined,
                RegisteredAt = participant?.RegisteredAt,
                JoinedAt = participant?.JoinedAt,
                CanRegister = contest.Status == ContestStatus.Published && !isRegistered,
                Canjoin = contest.Status == ContestStatus.Running && isRegistered && !hasJoined,
                CanStartVirtual = contest.Status == ContestStatus.Running && !isRegistered
            };
        }
    }
}