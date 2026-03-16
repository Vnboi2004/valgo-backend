using MediatR;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;
using VAlgo.Modules.ProblemManagement.Domain.Exceptions;
using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.ReorderHints
{
    public sealed class ReorderHintsCommandHandler : IRequestHandler<ReorderHintsCommand, Unit>
    {
        private readonly IProblemRepository _problemRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ReorderHintsCommandHandler(IProblemRepository problemRepository, IUnitOfWork unitOfWork)
        {
            _problemRepository = problemRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(ReorderHintsCommand request, CancellationToken cancellationToken)
        {
            var problem = await _problemRepository.GetByIdAsync(ProblemId.From(request.ProblemId), cancellationToken);

            if (problem == null)
                throw new ProblemNotFoundException(request.ProblemId);

            var ids = request.HintIds.Select(ProblemHintId.From).ToList();

            problem.ReorderHints(ids);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}