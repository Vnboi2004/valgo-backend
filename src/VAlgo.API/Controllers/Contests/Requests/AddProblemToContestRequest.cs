namespace VAlgo.API.Controllers.Contests.Requests
{
    public sealed record AddProblemToContestRequest(
        Guid ProblemId,
        string Code,
        int Points
    );
}