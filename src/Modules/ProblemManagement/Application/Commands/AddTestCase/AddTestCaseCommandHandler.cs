using MediatR;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;
using VAlgo.Modules.ProblemManagement.Domain.Exceptions;
using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.AddTestCase
{
    public sealed class AddTestCaseCommandHandler : IRequestHandler<AddTestCaseCommand, Unit>
    {
        private readonly IProblemRepository _problemRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddTestCaseCommandHandler(
            IProblemRepository problemRepository,
            IUnitOfWork unitOfWork
        )
        {
            _problemRepository = problemRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(AddTestCaseCommand request, CancellationToken cancellationToken)
        {
            var problemId = ProblemId.From(request.ProblemId);

            var problem = await _problemRepository.GetByIdAsync(problemId, cancellationToken);

            if (problem == null)
                throw new ProblemNotFoundException(request.ProblemId);

            problem.AddTestCase(
                request.Input,
                request.ExpectedOutput,
                request.OutputComparisonStrategy,
                request.IsSample
            );

            await _problemRepository.UpdateAsync(problem, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}