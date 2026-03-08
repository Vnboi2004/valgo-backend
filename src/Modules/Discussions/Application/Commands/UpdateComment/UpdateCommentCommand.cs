using MediatR;
using VAlgo.Modules.Discussions.Application.Interfaces;
using VAlgo.Modules.Discussions.Domain.ValueObjects;

namespace VAlgo.Modules.Discussions.Application.Commands.UpdateComment
{
    public sealed class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, Unit>
    {
        private readonly IDiscussionRepository _discussionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCommentCommandHandler(IDiscussionRepository discussionRepository, IUnitOfWork unitOfWork)
        {
            _discussionRepository = discussionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            var discussionId = DiscussionId.From(request.DiscussionId);

            var discussion = await _discussionRepository.GetByIdAsync(discussionId, cancellationToken);

            if (discussion == null)
                throw new InvalidOperationException("Discussion not found.");

            var comment = discussion.Comments.FirstOrDefault(x => x.Id.Value == request.CommentId);

            if (comment == null)
                throw new InvalidOperationException("Comment not found.");

            if (comment.AuthorId != request.UserId)
                throw new UnauthorizedAccessException("You cannot edit this comment");

            comment.UpdateContent(request.Content);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}