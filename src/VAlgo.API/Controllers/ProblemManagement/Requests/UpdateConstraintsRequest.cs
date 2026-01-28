namespace VAlgo.API.Controllers.ProblemManagement.Requests
{
    public sealed record UpdateConstraintsRequest
    (
        int TimeLimitMs,
        int MemoryLimitKb
    );
}