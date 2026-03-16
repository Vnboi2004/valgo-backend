namespace VAlgo.API.Controllers.ProblemManagement.Requests
{
    public sealed record UpdateProblemContentRequest(
        string Statement,
        string? Constraints,
        string? InputFormat,
        string? OutputFormat,
        string? FollowUp
    );
}