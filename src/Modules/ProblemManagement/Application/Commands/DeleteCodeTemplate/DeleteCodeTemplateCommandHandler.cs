using MediatR;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;
using VAlgo.Modules.ProblemManagement.Domain.Exceptions;
using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.DeleteCodeTemplate
{
    public sealed class DeleteCodeTemplateCommandHandler : IRequestHandler<DeleteCodeTemplateCommand, Unit>
    {
        private readonly IProblemRepository _problemRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCodeTemplateCommandHandler(
            IProblemRepository problemRepository,
            IUnitOfWork unitOfWork)
        {
            _problemRepository = problemRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteCodeTemplateCommand request, CancellationToken cancellationToken)
        {
            var problem = await _problemRepository.GetByIdAsync(ProblemId.From(request.ProblemId), cancellationToken)
                ?? throw new ProblemNotFoundException(request.ProblemId);

            problem.DeleteCodeTemplate(request.Language);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}