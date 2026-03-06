using MediatR;
using VAlgo.Modules.Contests.Application.Interfaces;
using VAlgo.Modules.Contests.Domain.Aggregates;

namespace VAlgo.Modules.Contests.Application.Commands.CreateContest
{
    public sealed class CreateContestCommandHandler : IRequestHandler<CreateContestCommand, Guid>
    {
        private readonly IContestRepository _contestRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateContestCommandHandler(IContestRepository contestRepository, IUnitOfWork unitOfWork)
        {
            _contestRepository = contestRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateContestCommand request, CancellationToken cancellationToken)
        {
            var contest = Contest.Create(
                request.Title,
                request.Description,
                request.StartTime,
                request.EndTime,
                request.CreatedBy
            );

            await _contestRepository.AddAsync(contest, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return contest.Id.Value;
        }
    }
}