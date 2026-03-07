namespace VAlgo.API.Controllers.Contests.Requests
{
    public sealed record ReorderContestProblemsRequest(
        IReadOnlyList<Guid> ProblemIds
    );
}