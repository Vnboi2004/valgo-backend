using MediatR;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;
using VAlgo.Modules.ProblemManagement.Domain.Exceptions;
using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;
using VAlgo.Modules.ProblemManagement.Infractructure.Persistence.Repositories;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.ReorderExamples
{
    public sealed class ReorderExamplesCommandHandler : IRequestHandler<ReorderExamplesCommand, Unit>
    {
        private readonly IProblemRepository _problemRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ReorderExamplesCommandHandler(IProblemRepository problemRepository, IUnitOfWork unitOfWork)
        {
            _problemRepository = problemRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(ReorderExamplesCommand request, CancellationToken cancellationToken)
        {
            var problem = await _problemRepository.GetByIdAsync(ProblemId.From(request.ProblemId), cancellationToken);

            if (problem == null)
                throw new ProblemNotFoundException(request.ProblemId);

            var ids = request.ExampleIds.Select(ProblemExampleId.From).ToList();

            problem.ReorderExamples(ids);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}