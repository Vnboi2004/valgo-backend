using MediatR;

namespace VAlgo.Modules.Contests.Application.Commands.ArchiveContest
{
    public sealed record ArchiveContestCommand(Guid ContestId) : IRequest<Unit>;
}