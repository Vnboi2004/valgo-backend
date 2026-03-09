using MediatR;
using VAlgo.Modules.Discussions.Application.Interfaces;
using VAlgo.Modules.Discussions.Domain.ValueObjects;

namespace VAlgo.Modules.Discussions.Application.Commands.DeleteComment
{
    public sealed class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, Unit>
    {
        private readonly IDiscussionRepository _discussionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCommentCommandHandler(IDiscussionRepository discussionRepository, IUnitOfWork unitOfWork)
        {
            _discussionRepository = discussionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var discussionId = DiscussionId.From(request.DiscussionId);

            var discussion = await _discussionRepository.GetByIdAsync(discussionId, cancellationToken);

            if (discussion == null)
                throw new InvalidOperationException("Discussion not found.");

            var comment = discussion.Comments.FirstOrDefault(x => x.Id.Value == request.CommentId);

            if (comment == null)
                throw new InvalidOperationException("Comment not found.");

            if (comment.AuthorId != request.UserId)
                throw new UnauthorizedAccessException("You cannot delete this comment.");

            discussion.RemoveComment(comment);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}