using MediatR;

namespace VAlgo.Modules.Discussions.Application.Commands.UpdateComment
{
    public sealed record UpdateCommentCommand(
        Guid DiscussionId,
        Guid CommentId,
        Guid UserId,
        string Content
    ) : IRequest<Unit>;
}