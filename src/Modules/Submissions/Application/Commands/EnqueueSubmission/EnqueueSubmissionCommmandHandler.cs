using MediatR;
using VAlgo.Modules.Submissions.Application.Abstractions;
using VAlgo.Modules.Submissions.Domain.Exceptions;
using VAlgo.Modules.Submissions.Domain.ValueObjects;

namespace VAlgo.Modules.Submissions.Application.Commands.EnqueueSubmission
{
    public sealed class EnqueueSubmissionCommandHandler : IRequestHandler<EnqueueSubmissionCommand, Unit>
    {
        private readonly ISubmissionRepository _submissionRepository;
        private readonly IJudgeQueue _judgeQueue;
        private readonly IUnitOfWork _unitOfWork;

        public EnqueueSubmissionCommandHandler(
            ISubmissionRepository submissionRepository,
            IJudgeQueue judgeQueue,
            IUnitOfWork unitOfWork
        )
        {
            _submissionRepository = submissionRepository;
            _judgeQueue = judgeQueue;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(EnqueueSubmissionCommand request, CancellationToken cancellationToken)
        {
            // 1. Load submission
            var submissionId = SubmissionId.From(request.SubmissionId);
            var submission = await _submissionRepository.GetByIdAsync(submissionId, cancellationToken);

            // 2. Check submission
            if (submission == null)
                throw new SubmissionDomainException("Submission not found");

            // 3. Domain transition
            var now = DateTime.UtcNow;
            submission.Enqueue(now);

            // 4. Persist state change 
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 5. Push job to JudgeWorker
            await _judgeQueue.EnqueueAsync(submission.Id.Value, cancellationToken);

            return Unit.Value;
        }
    }
}