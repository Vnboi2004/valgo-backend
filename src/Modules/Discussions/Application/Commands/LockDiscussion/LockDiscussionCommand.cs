using MediatR;

namespace VAlgo.Modules.Discussions.Application.Commands.LockDiscussion
{
    public sealed record LockDiscussionCommand(
        Guid DiscussionId,
        Guid UserId
    ) : IRequest<Unit>;
}