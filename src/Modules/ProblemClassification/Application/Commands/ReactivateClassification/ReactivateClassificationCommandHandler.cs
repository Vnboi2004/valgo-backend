using MediatR;
using VAlgo.Modules.ProblemClassification.Application.Abstractions;
using VAlgo.Modules.ProblemClassification.Application.Exceptions;
using VAlgo.Modules.ProblemClassification.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemClassification.Application.Commands.ReactivateClassification
{
    public sealed class ReactivateClassificationCommandHandler : IRequestHandler<ReactivateClassificationCommand, Unit>
    {
        private readonly IClassificationRepository _classificationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ReactivateClassificationCommandHandler(
            IClassificationRepository classificationRepository,
            IUnitOfWork unitOfWork
        )
        {
            _classificationRepository = classificationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(ReactivateClassificationCommand request, CancellationToken cancellationToken)
        {
            var classificationId = ClassificationId.From(request.ClassificationId);

            var classification = await _classificationRepository.GetByIdAsync(classificationId, cancellationToken);

            if (classification == null)
                throw new ClassificationNotFoundException(classificationId);

            classification.Reactivate();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}