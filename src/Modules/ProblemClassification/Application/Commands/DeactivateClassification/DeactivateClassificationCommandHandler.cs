using MediatR;
using VAlgo.Modules.ProblemClassification.Application.Abstractions;
using VAlgo.Modules.ProblemClassification.Application.Exceptions;
using VAlgo.Modules.ProblemClassification.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemClassification.Application.Commands.DeactivateClassification
{
    public sealed class DeactivateClassificationCommandHandler : IRequestHandler<DeactivateClassificationCommand, Unit>
    {
        private readonly IClassificationRepository _classificationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeactivateClassificationCommandHandler(
            IClassificationRepository classificationRepository,
            IUnitOfWork unitOfWork
        )
        {
            _classificationRepository = classificationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeactivateClassificationCommand request, CancellationToken cancellationToken)
        {
            var classificationId = ClassificationId.From(request.ClassificationId);

            var classification = await _classificationRepository.GetByIdAsync(classificationId, cancellationToken);

            if (classification == null)
                throw new ClassificationNotFoundException(classificationId);

            classification.Deactivate();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}