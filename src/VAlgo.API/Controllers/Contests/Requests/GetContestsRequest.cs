using VAlgo.Modules.Contests.Domain.Enums;

namespace VAlgo.API.Controllers.Contests.Requests
{
    public sealed record GetContestsRequest(
        ContestPhase? Phase,
        ContestVisibility? Visibility,
        int Page = 1,
        int PageSize = 20
    );
}