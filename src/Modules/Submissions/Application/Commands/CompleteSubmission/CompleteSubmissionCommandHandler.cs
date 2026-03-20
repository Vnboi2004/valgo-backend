using MediatR;
using VAlgo.Modules.Submissions.Application.Abstractions;
using VAlgo.Modules.Submissions.Domain.ValueObjects;
using VAlgo.SharedKernel.CrossModule.Submissions;
using VAlgo.SharedKernel.Time;

namespace VAlgo.Modules.Submissions.Application.Commands.CompleteSubmission
{
    public sealed class CompleteSubmissionCommandHandler : IRequestHandler<CompleteSubmissionCommand, Unit>
    {
        private readonly ISubmissionRepository _submissionRepository;
        private readonly IClock _clock;
        private readonly IUnitOfWork _unitOfWork;

        public CompleteSubmissionCommandHandler(
            ISubmissionRepository submissionRepository,
            IClock clock,
            IUnitOfWork unitOfWork
        )
        {
            _submissionRepository = submissionRepository;
            _clock = clock;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(CompleteSubmissionCommand request, CancellationToken cancellationToken)
        {
            if (request.Verdict == Verdict.Node)
                throw new InvalidOperationException("Verdict cannot be None when completing submission");

            var submissionId = SubmissionId.From(request.SubmissionId);
            var submission = await _submissionRepository.GetByIdAsync(submissionId, cancellationToken);

            if (submission == null)
                throw new InvalidOperationException($"Submission {request.SubmissionId} not found");

            var now = _clock.UtcNow;

            foreach (var tc in request.TestCases)
            {
                submission.AddTestCaseResult(
                    submissionId,
                    tc.Index,
                    tc.Verdict,
                    tc.TimeMs,
                    tc.MemoryKb,
                    tc.Output
                );
            }

            var judgeSummary = JudgeSummary.Create(
                request.TotalTestCases,
                request.PassedTestCases,
                request.TimeMs,
                request.MemoryKb
            );

            submission.Complete(request.Verdict, judgeSummary, now);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}