using MediatR;

namespace VAlgo.Modules.Contests.Application.Queries.GetContestParticipants
{
    public sealed record GetContestParticipantsQuery(Guid ContestId) : IRequest<IReadOnlyList<ContestParticipantDto>>;
}