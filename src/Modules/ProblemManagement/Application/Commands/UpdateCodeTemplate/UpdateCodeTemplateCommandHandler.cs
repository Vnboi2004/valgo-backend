using MediatR;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;
using VAlgo.Modules.ProblemManagement.Domain.Exceptions;
using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.UpdateCodeTemplate
{
    public sealed record UpdateCodeTemplateCommandHandler : IRequestHandler<UpdateCodeTemplateCommand, Unit>
    {
        private readonly IProblemRepository _problemRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCodeTemplateCommandHandler(IProblemRepository problemRepository, IUnitOfWork unitOfWork)
        {
            _problemRepository = problemRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateCodeTemplateCommand request, CancellationToken cancellationToken)
        {
            var problem = await _problemRepository.GetByIdAsync(ProblemId.From(request.ProblemId), cancellationToken)
                ?? throw new ProblemNotFoundException(request.ProblemId);

            problem.UpdateCodeTemplate(
                request.Language,
                request.UserTemplate,
                request.JudgeTemplate);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}