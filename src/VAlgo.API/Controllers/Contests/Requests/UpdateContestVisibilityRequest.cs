using VAlgo.Modules.Contests.Domain.Enums;

namespace VAlgo.API.Controllers.Contests.Requests
{
    public sealed record UpdateContestVisibilityRequest(
        ContestVisibility Visibility
    );
}