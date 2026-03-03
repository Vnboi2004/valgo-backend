using MediatR;
using VAlgo.Modules.Submissions.Application.Abstractions;
using VAlgo.Modules.Submissions.Domain.Exceptions;
using VAlgo.Modules.Submissions.Domain.ValueObjects;
using VAlgo.SharedKernel.Time;

namespace VAlgo.Modules.Submissions.Application.Commands.StartSubmissionExecution
{
    public sealed class StartSubmissionExecutionCommandHandler : IRequestHandler<StartSubmissionExecutionCommand, Unit>
    {
        private readonly ISubmissionRepository _submissionRepository;
        private readonly IClock _clock;
        private readonly IUnitOfWork _unitOfWork;

        public StartSubmissionExecutionCommandHandler(
            ISubmissionRepository submissionRepository,
            IClock clock,
            IUnitOfWork unitOfWork
        )
        {
            _submissionRepository = submissionRepository;
            _clock = clock;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(StartSubmissionExecutionCommand request, CancellationToken cancellationToken)
        {
            var submissionId = SubmissionId.From(request.SubmissionId);
            var submission = await _submissionRepository.GetByIdAsync(submissionId, cancellationToken);

            if (submission == null)
                throw new SubmissionDomainException("Submission not found");

            var now = _clock.UtcNow;
            submission.StartRunning(now);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}