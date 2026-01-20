using MediatR;
using VAlgo.Modules.Submissions.Application.Abstractions;
using VAlgo.Modules.Submissions.Domain.Aggregates;
using VAlgo.Modules.Submissions.Domain.ValueObjects;

namespace VAlgo.Modules.Submissions.Application.Commands.CreateSubmission
{
    public sealed class CreateSubmissionCommandHandler : IRequestHandler<CreateSubmissionCommand, Guid>
    {
        private readonly ISubmissionRepository _submissionRepository;
        private readonly IUserReadService _userReadService;
        private readonly IProblemReadService _problemReadService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSubmissionCommandHandler(
            ISubmissionRepository submissionRepository,
            IUserReadService userReadService,
            IProblemReadService problemReadService,
            IUnitOfWork unitOfWork
        )
        {
            _submissionRepository = submissionRepository;
            _userReadService = userReadService;
            _problemReadService = problemReadService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateSubmissionCommand request, CancellationToken cancellationToken)
        {
            // 1. Check user
            var existUser = await _userReadService.ExistsAsync(request.UserId);
            if (!existUser)
                throw new ApplicationException("User does not exist");

            // 2. Check problem
            var existProblem = await _problemReadService.ExistsAsync(request.ProblemId);
            if (!existProblem)
                throw new ApplicationException("Problem does not exist");

            // 3. Convert field
            var now = DateTime.UtcNow;
            var language = new Language(request.Language);

            // 4. Create Submission
            var submission = Submission.Create(
                request.UserId,
                request.ProblemId,
                language,
                request.SourceCode,
                1000,
                256000,
                now
            );

            await _submissionRepository.AddAsync(submission, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return submission.Id.Value;
        }
    }
}