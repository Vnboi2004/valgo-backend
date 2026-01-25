using VAlgo.Modules.ProblemManagement.Domain.Enums;

namespace VAlgo.API.Controllers.ProblemManagement.Requests
{
    public sealed record CreateProblemRequest(
        string Code,
        string Title,
        string Statement,
        string? ShortDescription,
        Difficulty Difficulty,
        int TimeLimitMs,
        int MemoryLimitKb
    );
}