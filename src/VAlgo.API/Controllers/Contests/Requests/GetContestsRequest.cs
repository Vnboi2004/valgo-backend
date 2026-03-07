using VAlgo.Modules.Contests.Domain.Enums;

namespace VAlgo.API.Controllers.Contests.Requests
{
    public sealed record GetContestsRequest(
        ContestStatus? Status,
        ContestVisibility? Visibility,
        int Page,
        int PageSize
    );
}