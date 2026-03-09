using MediatR;

namespace VAlgo.Modules.Discussions.Application.Commands.CreateDiscussion
{
    public sealed record CreateDiscussionCommand(
        Guid ProblemId,
        Guid AuthorId,
        string Title,
        string Content
    ) : IRequest<Guid>;
}