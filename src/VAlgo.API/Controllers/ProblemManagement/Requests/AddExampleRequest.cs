namespace VAlgo.API.Controllers.ProblemManagement.Requests
{
    public sealed record AddExampleRequest(
        string Input,
        string Output,
        string? Explanation
    );
}