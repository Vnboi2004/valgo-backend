using MediatR;
using VAlgo.Modules.Contests.Application.Interfaces;
using VAlgo.Modules.Contests.Domain.ValueObjects;

namespace VAlgo.Modules.Contests.Application.Commands.AddProblemToContest
{
    public sealed class AddProblemToContestCommandHandler : IRequestHandler<AddProblemToContestCommand, Guid>
    {
        private readonly IContestRepository _contestRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddProblemToContestCommandHandler(IContestRepository contestRepository, IUnitOfWork unitOfWork)
        {
            _contestRepository = contestRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(AddProblemToContestCommand request, CancellationToken cancellationToken)
        {
            var contestId = ContestId.From(request.ContestId);

            var contest = await _contestRepository.GetByIdAsync(contestId, cancellationToken);

            if (contest == null)
                throw new InvalidOperationException("Contest not found.");

            contest.AddProblem(request.ProblemId, request.Code, request.Points);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return request.ProblemId;
        }
    }
}