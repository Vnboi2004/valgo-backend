namespace VAlgo.API.Controllers.Contests.Requests
{
    public sealed record UpdateContestScheduleRequest(
        DateTime StartTime,
        DateTime EndTime
    );
}