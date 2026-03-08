using MediatR;

namespace VAlgo.Modules.Discussions.Application.Commands.AddComment
{
    public sealed record AddCommentCommand(
        Guid DiscussionId,
        Guid AuthorId,
        string Content,
        Guid? ParentCommentId
    ) : IRequest<Guid>;
}