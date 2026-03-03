using MediatR;
using VAlgo.Modules.ProblemClassification.Application.Abstractions;
using VAlgo.Modules.ProblemClassification.Application.Exceptions;
using VAlgo.Modules.ProblemClassification.Domain.ValueObjects;

namespace VAlgo.Modules.ProblemClassification.Application.Commands.RenameClassification
{
    public sealed class RenameClassificationCommandHandler : IRequestHandler<RenameClassificationCommand, Unit>
    {
        private readonly IClassificationRepository _classificationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RenameClassificationCommandHandler(
            IClassificationRepository classificationRepository,
            IUnitOfWork unitOfWork
        )
        {
            _classificationRepository = classificationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(RenameClassificationCommand request, CancellationToken cancellationToken)
        {
            var classificationId = ClassificationId.From(request.ClassificationId);

            var classification = await _classificationRepository.GetByIdAsync(classificationId, cancellationToken);

            if (classification == null)
                throw new ClassificationNotFoundException(classificationId);

            classification.Rename(request.NewName);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}