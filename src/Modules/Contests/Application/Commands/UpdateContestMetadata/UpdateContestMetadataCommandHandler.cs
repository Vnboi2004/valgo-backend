using MediatR;
using VAlgo.Modules.Contests.Application.Interfaces;
using VAlgo.Modules.Contests.Domain.ValueObjects;

namespace VAlgo.Modules.Contests.Application.Commands.UpdateContestMetadata
{
    public sealed class UpdateContestMetadataCommandHandler : IRequestHandler<UpdateContestMetadataCommand, Unit>
    {
        private readonly IContestRepository _contestRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateContestMetadataCommandHandler(IContestRepository contestRepository, IUnitOfWork unitOfWork)
        {
            _contestRepository = contestRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateContestMetadataCommand request, CancellationToken cancellationToken)
        {
            var contestId = ContestId.From(request.ContestId);
            var contest = await _contestRepository.GetByIdAsync(contestId, cancellationToken);

            if (contest == null)
                throw new InvalidOperationException("Contest not found.");

            contest.UpdateMetadata(request.Title, request.Description);

            await _contestRepository.UpdateAsync(contest, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}