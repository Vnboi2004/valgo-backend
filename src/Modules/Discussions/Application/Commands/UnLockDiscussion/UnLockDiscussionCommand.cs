using MediatR;

namespace VAlgo.Modules.Discussions.Application.Commands.UnLockDiscussion
{
    public sealed record UnLockDiscussionCommand(
        Guid DiscussionId,
        Guid UserId
    ) : IRequest<Unit>;
}