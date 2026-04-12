using MediatR;
using VAlgo.Modules.Contests.Application.Interfaces;
using VAlgo.Modules.Contests.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Contests.Application.Commands.UnregisterContest
{
    public sealed class UnregisterContestCommandHandler : IRequestHandler<UnregisterContestCommand, Unit>
    {
        private readonly IContestRepository _contestRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public UnregisterContestCommandHandler(IContestRepository contestRepository, ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
        {
            _contestRepository = contestRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UnregisterContestCommand request, CancellationToken cancellationToken)
        {
            var contest = await _contestRepository.GetByIdAsync(ContestId.From(request.ContestId), cancellationToken);
            if (contest is null)
            {
                throw new InvalidOperationException($"Contest with ID {request.ContestId} not found.");
            }

            contest.Unregister(_currentUserService.UserId);

            await _contestRepository.UpdateAsync(contest, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}