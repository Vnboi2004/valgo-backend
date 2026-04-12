using MediatR;
using VAlgo.Modules.Contests.Application.Interfaces;
using VAlgo.Modules.Contests.Domain.Entities;
using VAlgo.Modules.Contests.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Contests.Application.Commands.StartVirtualContest
{
    public sealed class StartVirtualContestCommandHandler : IRequestHandler<StartVirtualContestCommand, Guid>
    {
        private readonly IContestRepository _contestRepository;
        private readonly IVirtualContestSessionRepository _virtualContestSessionRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public StartVirtualContestCommandHandler(
            IContestRepository contestRepository,
            IVirtualContestSessionRepository virtualContestSessionRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork
        )
        {
            _contestRepository = contestRepository;
            _virtualContestSessionRepository = virtualContestSessionRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(StartVirtualContestCommand request, CancellationToken cancellationToken)
        {
            var contest = await _contestRepository.GetByIdAsync(ContestId.From(request.ContestId), cancellationToken);
            if (contest == null)
                throw new InvalidOperationException("Contest not found.");

            if (!contest.AllowVirtual)
                throw new InvalidOperationException("Virtual contest not allowed.");

            var userId = _currentUserService.UserId;
            var existed = await _virtualContestSessionRepository.ExistsAsync(request.ContestId, userId);
            if (existed)
                throw new InvalidOperationException("User already started virtual contest.");

            var session = VirtualContestSession.Create(
                contest.Id,
                userId,
                DateTime.UtcNow,
                contest.Duration,
                contest.Problems.Select(p => p.ProblemId)
            );

            await _virtualContestSessionRepository.AddAsync(session, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return session.Id.Value;
        }
    }
}