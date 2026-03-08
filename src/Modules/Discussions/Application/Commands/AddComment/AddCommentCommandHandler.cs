using MediatR;
using VAlgo.Modules.Discussions.Application.Interfaces;
using VAlgo.Modules.Discussions.Domain.ValueObjects;

namespace VAlgo.Modules.Discussions.Application.Commands.AddComment
{
    public sealed class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, Guid>
    {
        private readonly IDiscussionRepository _discussionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddCommentCommandHandler(IDiscussionRepository discussionRepository, IUnitOfWork unitOfWork)
        {
            _discussionRepository = discussionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            var discussionId = DiscussionId.From(request.DiscussionId);

            var discussion = await _discussionRepository.GetByIdAsync(discussionId, cancellationToken);

            CommentId? parentId = null;

            if (request.ParentCommentId.HasValue)
                parentId = CommentId.From(request.ParentCommentId.Value);

            if (discussion == null)
                throw new InvalidOperationException("Dicussion not found.");

            var comment = discussion.AddComment(request.AuthorId, request.Content, parentId);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return comment.Id.Value;
        }
    }
}