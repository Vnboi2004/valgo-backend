namespace VAlgo.API.Controllers.Problems.Requests
{
    public sealed record UpdateConstraintsRequest
    (
        int TimeLimitMs,
        int MemoryLimitKb
    );
}