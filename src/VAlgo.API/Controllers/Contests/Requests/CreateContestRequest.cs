using VAlgo.Modules.Contests.Domain.Enums;

namespace VAlgo.API.Controllers.Contests.Requests
{
    public sealed record CreateContestsRequest(
        string Title,
        string Description,
        DateTime StartTime,
        DateTime EndTime,
        ContestVisibility Visibility,
        Guid CreatedBy,
        int? MaxParticipants
    );
}