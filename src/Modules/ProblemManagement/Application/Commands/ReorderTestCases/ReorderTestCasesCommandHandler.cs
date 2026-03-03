using MediatR;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;
using VAlgo.Modules.ProblemManagement.Domain.Exceptions;
using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.ReorderTestCases
{
    public sealed class ReorderTestCasesCommandHandler : IRequestHandler<ReorderTestCasesCommand, Unit>
    {
        private readonly IProblemRepository _problemRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ReorderTestCasesCommandHandler(
            IProblemRepository problemRepository,
            IUnitOfWork unitOfWork
        )
        {
            _problemRepository = problemRepository;
            _unitOfWork = unitOfWork;
        }


        public async Task<Unit> Handle(ReorderTestCasesCommand request, CancellationToken cancellationToken)
        {
            var problemId = ProblemId.From(request.ProblemId);

            var problem = await _problemRepository.GetByIdAsync(problemId, cancellationToken);

            if (problem == null)
                throw new ProblemNotFoundException(request.ProblemId);

            var ids = request.OrderedTestCaseIds.Select(TestCaseId.From).ToList();

            problem.ReorderTestCases(ids);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}