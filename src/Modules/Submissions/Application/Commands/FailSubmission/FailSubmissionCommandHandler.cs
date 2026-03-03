using MediatR;
using VAlgo.Modules.Submissions.Application.Abstractions;
using VAlgo.Modules.Submissions.Domain.ValueObjects;
using VAlgo.SharedKernel.Time;

namespace VAlgo.Modules.Submissions.Application.Commands.FailSubmission
{
    public sealed class FailSubmissionCommandHandler : IRequestHandler<FailSubmissionCommand, Unit>
    {
        private readonly ISubmissionRepository _submissionRepository;
        private readonly IClock _clock;
        private readonly IUnitOfWork _unitOfWork;

        public FailSubmissionCommandHandler(
            ISubmissionRepository submissionRepository,
            IClock clock,
            IUnitOfWork unitOfWork
        )
        {
            _submissionRepository = submissionRepository;
            _clock = clock;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(FailSubmissionCommand request, CancellationToken cancellationToken)
        {
            var submissionId = SubmissionId.From(request.SubmissionId);

            var submission = await _submissionRepository.GetByIdAsync(submissionId, cancellationToken);

            if (submission == null)
                throw new InvalidOperationException($"Submission {request.SubmissionId} not found");

            var now = _clock.UtcNow;

            submission.Fail(request.Reason, now);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}