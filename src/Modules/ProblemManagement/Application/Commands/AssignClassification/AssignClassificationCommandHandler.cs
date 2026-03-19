using MediatR;
using VAlgo.Modules.ProblemClassification.Domain.ValueObjects;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;
using VAlgo.Modules.ProblemManagement.Domain.Exceptions;
using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.AssignClassification
{
    public sealed class AssignClassificationCommandHandler : IRequestHandler<AssignClassificationCommand, Unit>
    {
        private readonly IProblemRepository _problemRepository;
        private readonly IClassificationRepository _classificationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AssignClassificationCommandHandler(
            IProblemRepository problemRepository,
            IClassificationRepository classificationRepository,
            IUnitOfWork unitOfWork
        )
        {
            _problemRepository = problemRepository;
            _classificationRepository = classificationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(AssignClassificationCommand request, CancellationToken cancellationToken)
        {
            var problemId = ProblemId.From(request.ProblemId);

            var problem = await _problemRepository.GetByIdAsync(problemId, cancellationToken);

            if (problem == null)
                throw new ProblemNotFoundException(request.ProblemId);

            var classification = await _classificationRepository.GetByIdAsync(ClassificationId.From(request.ClassificationId), cancellationToken);

            if (classification == null)
                throw new ClassificationNotFoundException(request.ClassificationId);

            problem.AddClassificationRef(classification);

            // await _problemRepository.UpdateAsync(problem, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}