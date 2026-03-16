namespace VAlgo.API.Controllers.ProblemManagement.Requests
{
    public sealed record UpdateExampleRequest(
        string Input,
        string Output,
        string? Explanation
    );
}