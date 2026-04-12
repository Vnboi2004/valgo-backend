using VAlgo.Modules.Contests.Domain.Enums;

namespace VAlgo.API.Controllers.Contests.Requests
{
    public sealed record CreateContestsRequest(
        string Title,
        string Description,
        string Code,
        DateTime StartTime,
        DateTime EndTime,
        ContestVisibility Visibility,
        ContestType Type
    );
}