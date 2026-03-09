using MediatR;

namespace VAlgo.Modules.Contests.Application.Commands.PublishContest
{
    public sealed record PublishContestCommand(Guid ContestId) : IRequest<Unit>;
}