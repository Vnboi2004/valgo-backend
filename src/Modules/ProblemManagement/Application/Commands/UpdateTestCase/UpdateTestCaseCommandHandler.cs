using MediatR;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;
using VAlgo.Modules.ProblemManagement.Domain.Aggregates;
using VAlgo.Modules.ProblemManagement.Domain.Exceptions;
using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.UpdateTestCase
{
    public sealed class UpdateTestCaseCommandHandler : IRequestHandler<UpdateTestCaseCommand, Unit>
    {
        private readonly IProblemRepository _problemRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTestCaseCommandHandler(IProblemRepository problemRepository, IUnitOfWork unitOfWork)
        {
            _problemRepository = problemRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateTestCaseCommand request, CancellationToken cancellationToken)
        {
            var problem = await _problemRepository.GetByIdAsync(ProblemId.From(request.ProblemId), cancellationToken);

            if (problem == null)
                throw new ProblemNotFoundException(request.ProblemId);

            problem.UpdateTestCase(
                TestCaseId.From(request.TestCaseId), request.Input,
                request.ExpectedOutput,
                request.OutputComparisonStrategy,
                request.IsSample
            );

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}