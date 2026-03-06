using MediatR;
using VAlgo.Modules.Contests.Application.Interfaces;
using VAlgo.Modules.Contests.Domain.ValueObjects;

namespace VAlgo.Modules.Contests.Application.Commands.UpdateContestMaxParticipants
{
    public sealed class UpdateContestMaxParticipantsCommandHandler : IRequestHandler<UpdateContestMaxParticipantsCommand, Unit>
    {
        private readonly IContestRepository _contestRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateContestMaxParticipantsCommandHandler(IContestRepository contestRepository, IUnitOfWork unitOfWork)
        {
            _contestRepository = contestRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateContestMaxParticipantsCommand request, CancellationToken cancellationToken)
        {
            var contestId = ContestId.From(request.ContestId);
            var contest = await _contestRepository.GetByIdAsync(contestId, cancellationToken);

            if (contest == null)
                throw new InvalidOperationException("Contest not found.");

            contest.UpdateMaxParticipants(request.MaxParticipants);

            await _contestRepository.UpdateAsync(contest, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;                                                                                                                          
        }
    }
}