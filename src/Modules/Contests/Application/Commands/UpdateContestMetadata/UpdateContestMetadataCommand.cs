using MediatR;

namespace VAlgo.Modules.Contests.Application.Commands.UpdateContestMetadata
{
    public sealed record UpdateContestMetadataCommand(
        Guid ContestId,
        string Title,
        string Description
    ) : IRequest<Unit>;
}