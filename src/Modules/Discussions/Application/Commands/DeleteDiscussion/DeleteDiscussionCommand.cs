using MediatR;

namespace VAlgo.Modules.Discussions.Application.Commands.DeleteDiscussion
{
    public sealed record DeleteDiscussionCommand(
        Guid DiscussionId,
        Guid UserId
    ) : IRequest<Unit>;
}