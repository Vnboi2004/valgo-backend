using MediatR;

namespace VAlgo.Modules.Discussions.Application.Commands.UpdateDiscussion
{
    public sealed record UpdateDiscussionCommand(
        Guid DiscussionId,
        Guid UserId,
        string Title,
        string Content
    ) : IRequest<Unit>;
}