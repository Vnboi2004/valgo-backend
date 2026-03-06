using MediatR;
using VAlgo.Modules.Contests.Application.Interfaces;
using VAlgo.Modules.Contests.Domain.ValueObjects;

namespace VAlgo.Modules.Contests.Application.Commands.ReorderContestProblems
{
    public sealed class ReorderContestProblemsCommandHandler : IRequestHandler<ReorderContestProblemsCommand, Unit>
    {
        private readonly IContestRepository _contestRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ReorderContestProblemsCommandHandler(IContestRepository contestRepository, IUnitOfWork unitOfWork)
        {
            _contestRepository = contestRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(ReorderContestProblemsCommand request, CancellationToken cancellationToken)
        {
            var contestId = ContestId.From(request.ContestId);

            var contest = await _contestRepository.GetByIdAsync(contestId, cancellationToken);
            if (contest == null)
                throw new InvalidOperationException("Contest not found.");

            contest.ReorderProblems(request.ProblemIds);

            await _contestRepository.UpdateAsync(contest, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}