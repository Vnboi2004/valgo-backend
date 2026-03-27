using MediatR;
using VAlgo.Modules.Submissions.Application.Abstractions;
using VAlgo.Modules.Submissions.Application.Events;
using VAlgo.Modules.Submissions.Domain.Aggregates;
using VAlgo.Modules.Submissions.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;
using VAlgo.SharedKernel.Messaging;

namespace VAlgo.Modules.Submissions.Application.Commands.CreateSubmission
{
    public sealed class CreateSubmissionCommandHandler : IRequestHandler<CreateSubmissionCommand, Guid>
    {
        private readonly ISubmissionRepository _submissionRepository;
        private readonly IUserReadService _userReadService;
        private readonly IProblemReadToSubmissionService _problemReadToSubmissionService;
        private readonly IRabbitMqPublisher _publisher;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSubmissionCommandHandler(
            ISubmissionRepository submissionRepository,
            IUserReadService userReadService,
            IProblemReadToSubmissionService problemReadToSubmissionService,
            IRabbitMqPublisher publisher,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork
        )
        {
            _submissionRepository = submissionRepository;
            _userReadService = userReadService;
            _problemReadToSubmissionService = problemReadToSubmissionService;
            _publisher = publisher;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateSubmissionCommand request, CancellationToken cancellationToken)
        {
            // 1. Check user
            var existUser = await _userReadService.ExistsAsync(_currentUserService.UserId);
            if (!existUser)
                throw new ApplicationException("User does not exist");

            // 2.Check problem
            var existProblem = await _problemReadToSubmissionService.ExistsAsync(request.ProblemId);
            if (!existProblem)
                throw new ApplicationException("Problem does not exist");

            // 3. Convert field
            var now = DateTime.UtcNow;
            var language = new Language(request.Language);

            // 4. Create Submission
            var submission = Submission.Create(
                _currentUserService.UserId,
                request.ProblemId,
                request.ContestId,
                language,
                request.SourceCode,
                now
            );

            submission.Enqueue(now);

            await _submissionRepository.AddAsync(submission, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _publisher.PublishAsync(
                queue: "judge.submission",
                new SubmissionQueuedIntegrationEvent(submission.Id.Value, submission.ProblemId),
                cancellationToken
            );

            return submission.Id.Value;
        }
    }
}