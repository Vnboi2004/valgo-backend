using MediatR;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<CompleteSubmissionCommandHandler> _logger;

        public CompleteSubmissionCommandHandler(
            ISubmissionRepository submissionRepository,
            IClock clock,
            IUnitOfWork unitOfWork,
            ILogger<CompleteSubmissionCommandHandler> logger
        )
        {
            _submissionRepository = submissionRepository;
            _clock = clock;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Unit> Handle(CompleteSubmissionCommand request, CancellationToken cancellationToken)
        {
            if (request.Verdict == Verdict.Node)
                throw new InvalidOperationException("Verdict cannot be None when completing submission");

            _logger.LogInformation("Start CompleteSubmission: {SubmissionId}", request.SubmissionId);

            var submissionId = SubmissionId.From(request.SubmissionId);
            var submission = await _submissionRepository.GetByIdAsync(submissionId, cancellationToken);

            if (submission == null)
                throw new InvalidOperationException($"Submission {request.SubmissionId} not found");

            var now = _clock.UtcNow;

            try
            {
                _logger.LogInformation("Step 1: Add test cases");

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

                _logger.LogInformation("Step 2: Create summary");

                var judgeSummary = JudgeSummary.Create(
                    request.TotalTestCases,
                    request.PassedTestCases,
                    request.MaxTimeMs,
                    request.MaxMemoryKb
                );

                _logger.LogInformation("Step 3: Complete submission");
                submission.Complete(request.Verdict, judgeSummary, now);

                _logger.LogInformation("Step 4: Save changes");
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("CompleteSubmission SUCCESS");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CompleteSubmission FAILED at step");
                throw;
            }

            return Unit.Value;
        }
    }
}