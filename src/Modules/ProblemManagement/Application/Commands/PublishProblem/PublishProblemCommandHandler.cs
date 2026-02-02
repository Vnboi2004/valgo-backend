using MediatR;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;
using VAlgo.Modules.ProblemManagement.Domain.Exceptions;
using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;
using VAlgo.SharedKernel.Time;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.PublishProblem
{
    public sealed class PublishProblemCommandHandler : IRequestHandler<PublishProblemCommand, Unit>
    {
        private readonly IProblemRepository _problemRepository;
        private readonly IClock _clock;
        private readonly IUnitOfWork _unitOfWork;

        public PublishProblemCommandHandler(
            IProblemRepository problemRepository,
            IClock clock,
            IUnitOfWork unitOfWork
        )
        {
            _problemRepository = problemRepository;
            _clock = clock;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(PublishProblemCommand request, CancellationToken cancellationToken)
        {
            var problemId = ProblemId.From(request.ProblemId);

            var problem = await _problemRepository.GetByIdAsync(problemId, cancellationToken);

            if (problem == null)
                throw new ProblemNotFoundException(request.ProblemId);

            var now = _clock.UtcNow;

            problem.Publish(now);

            await _problemRepository.UpdateAsync(problem, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}