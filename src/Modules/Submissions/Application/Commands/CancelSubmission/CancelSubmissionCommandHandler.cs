using MediatR;
using VAlgo.Modules.Submissions.Application.Abstractions;
using VAlgo.Modules.Submissions.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;
using VAlgo.SharedKernel.Time;

namespace VAlgo.Modules.Submissions.Application.Commands.CancelSubmission
{
    public sealed class CancelSubmissionCommandHandler : IRequestHandler<CancelSubmissionCommand, Unit>
    {
        private readonly ISubmissionRepository _submissionRepository;
        private readonly IClock _clock;
        private readonly IUnitOfWork _unitOfWork;

        public CancelSubmissionCommandHandler(
            ISubmissionRepository submissionRepository,
            IClock clock,
            IUnitOfWork unitOfWork
        )
        {
            _submissionRepository = submissionRepository;
            _clock = clock;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(CancelSubmissionCommand request, CancellationToken cancellationToken)
        {
            var submissionId = SubmissionId.From(request.SubmissionId);

            var submission = await _submissionRepository.GetByIdAsync(submissionId, cancellationToken);

            if (submission == null)
                throw new InvalidOperationException($"Submission {request.SubmissionId} not found");

            var now = _clock.UtcNow;

            submission.Cancel(now);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}