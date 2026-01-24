using MediatR;
using VAlgo.Modules.ProblemClassification.Application.Abstractions;
using VAlgo.Modules.ProblemClassification.Application.Exceptions;
using VAlgo.Modules.ProblemClassification.Domain.Aggregates;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemClassification.Application.Commands.CreateClassification
{
    public sealed class CreateClassificationCommandHandler : IRequestHandler<CreateClassificationCommand, Guid>
    {
        private readonly IProblemClassificationRepository _problemClassificationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateClassificationCommandHandler(
            IProblemClassificationRepository problemClassificationRepository,
            IUnitOfWork unitOfWork
        )
        {
            _problemClassificationRepository = problemClassificationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateClassificationCommand request, CancellationToken cancellationToken)
        {
            var exists = await _problemClassificationRepository.ExistsByCodeAsync(request.Code, cancellationToken);
            if (exists)
                throw new ClassificationCodeAlreadyExistsException(request.Code);

            var classification = Classification.Create(
                request.Code,
                request.Name,
                request.Type
            );

            await _problemClassificationRepository.AddAsync(classification, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return classification.Id.Value;
        }
    }
}