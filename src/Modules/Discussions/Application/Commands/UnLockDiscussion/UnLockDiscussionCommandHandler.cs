using MediatR;
using VAlgo.Modules.Discussions.Application.Interfaces;
using VAlgo.Modules.Discussions.Domain.ValueObjects;

namespace VAlgo.Modules.Discussions.Application.Commands.UnLockDiscussion
{
    public sealed class UnLockDiscussionCommandHandler : IRequestHandler<UnLockDiscussionCommand, Unit>
    {
        private readonly IDiscussionRepository _discussionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UnLockDiscussionCommandHandler(IDiscussionRepository discussionRepository, IUnitOfWork unitOfWork)
        {
            _discussionRepository = discussionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UnLockDiscussionCommand request, CancellationToken cancellationToken)
        {
            var discussionId = DiscussionId.From(request.DiscussionId);

            var discussion = await _discussionRepository.GetByIdAsync(discussionId, cancellationToken);

            if (discussion == null)
                throw new InvalidOperationException("Dicussion not found.");

            if (discussion.AuthorId != request.UserId)
                throw new UnauthorizedAccessException("You cannot unlock this discussion.");

            discussion.UnLock();

            await _discussionRepository.UpdateAsync(discussion, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}