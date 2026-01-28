using MediatR;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;
using VAlgo.Modules.ProblemManagement.Domain.Aggregates;
using VAlgo.Modules.ProblemManagement.Domain.Exceptions;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.CreateProblem
{
    public sealed class CreateProblemCommandHandler : IRequestHandler<CreateProblemCommand, Guid>
    {
        private readonly IProblemRepository _problemRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProblemCommandHandler(
            IProblemRepository problemRepository,
            IUnitOfWork unitOfWork
        )
        {
            _problemRepository = problemRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateProblemCommand request, CancellationToken cancellationToken)
        {
            var normalizedCode = request.Code.Trim().ToUpperInvariant();

            var existCode = await _problemRepository.ExistsByCodeAsync(normalizedCode, cancellationToken);
            if (existCode)
                throw new ProblemCodeAlreadyExistsException(normalizedCode);

            var problem = Problem.Create(
                request.Code,
                request.Title,
                request.Statement,
                request.ShortDescription,
                request.Difficulty,
                request.TimeLimitMs,
                request.MemoryLimitKb
            );

            await _problemRepository.AddAsync(problem, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return problem.Id.Value;
        }
    }
}