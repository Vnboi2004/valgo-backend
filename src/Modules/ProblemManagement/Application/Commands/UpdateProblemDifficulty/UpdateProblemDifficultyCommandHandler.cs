using MediatR;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;
using VAlgo.Modules.ProblemManagement.Domain.Exceptions;
using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.UpdateProblemDifficulty
{
    public sealed class UpdateProblemDifficultyCommandHandler : IRequestHandler<UpdateProblemDifficultyCommand, Unit>
    {
        private readonly IProblemRepository _problemRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProblemDifficultyCommandHandler(
            IProblemRepository problemRepository,
            IUnitOfWork unitOfWork
        )
        {
            _problemRepository = problemRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateProblemDifficultyCommand request, CancellationToken cancellationToken)
        {
            var problemId = ProblemId.From(request.ProblemId);

            var problem = await _problemRepository.GetByIdAsync(problemId, cancellationToken);

            if (problem == null)
                throw new ProblemNotFoundException(request.ProblemId);

            problem.UpdateDifficulty(request.Difficulty);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}