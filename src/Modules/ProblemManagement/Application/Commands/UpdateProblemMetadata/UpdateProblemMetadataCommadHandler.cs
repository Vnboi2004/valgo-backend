using MediatR;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;
using VAlgo.Modules.ProblemManagement.Domain.Exceptions;
using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.UpdateProblemMetadata
{
    public sealed class UpdateProblemMetadataCommandHandler : IRequestHandler<UpdateProblemMetadataCommand, Unit>
    {
        private readonly IProblemRepository _problemRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProblemMetadataCommandHandler(
            IProblemRepository problemRepository,
            IUnitOfWork unitOfWork
        )
        {
            _problemRepository = problemRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateProblemMetadataCommand request, CancellationToken cancellationToken)
        {
            var problemId = ProblemId.From(request.ProblemId);

            var problem = await _problemRepository.GetByIdAsync(problemId, cancellationToken);

            if (problem == null)
                throw new ProblemNotFoundException(request.ProblemId);

            problem.UpdateMetadata(request.Title, request.Statement, request.ShortDescription);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}