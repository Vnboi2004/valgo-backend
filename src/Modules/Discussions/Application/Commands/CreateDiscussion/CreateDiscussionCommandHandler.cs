using MediatR;
using VAlgo.Modules.Discussions.Application.Interfaces;
using VAlgo.Modules.Discussions.Domain.Aggregates;

namespace VAlgo.Modules.Discussions.Application.Commands.CreateDiscussion
{
    public sealed class CreateDiscussionCommandHandler : IRequestHandler<CreateDiscussionCommand, Guid>
    {
        private readonly IDiscussionRepository _discussionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateDiscussionCommandHandler(IDiscussionRepository discussionRepository, IUnitOfWork unitOfWork)
        {
            _discussionRepository = discussionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateDiscussionCommand request, CancellationToken cancellationToken)
        {
            var discussion = Discussion.Create(
                request.ProblemId,
                request.AuthorId,
                request.Title,
                request.Content
            );

            await _discussionRepository.AddAsync(discussion, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return discussion.Id.Value;
        }
    }
}