using MediatR;

namespace VAlgo.Modules.Discussions.Application.Commands.DeleteComment
{
    public sealed record DeleteCommentCommand(
        Guid DiscussionId,
        Guid CommentId,
        Guid UserId
    ) : IRequest<Unit>;
}