using MediatR;
using VAlgo.Modules.ProblemClassification.Application.Abstractions;
using VAlgo.Modules.ProblemClassification.Application.Exceptions;
using VAlgo.Modules.ProblemClassification.Domain.Aggregates;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemClassification.Application.Commands.CreateClassification
{
    public sealed class CreateClassificationCommandHandler : IRequestHandler<CreateClassificationCommand, Guid>
    {
        private readonly IClassificationRepository _classificationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateClassificationCommandHandler(
            IClassificationRepository classificationRepository,
            IUnitOfWork unitOfWork
        )
        {
            _classificationRepository = classificationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateClassificationCommand request, CancellationToken cancellationToken)
        {
            var exists = await _classificationRepository.ExistsByCodeAsync(request.Code, cancellationToken);
            if (exists)
                throw new ClassificationCodeAlreadyExistsException(request.Code);

            var classification = Classification.Create(
                request.Code,
                request.Name,
                request.Type
            );

            await _classificationRepository.AddAsync(classification, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return classification.Id.Value;
        }
    }
}