namespace VAlgo.API.Controllers.Submissions.Requests
{
    public sealed record RunCodeRequest(
        Guid ProblemId,
        string Language,
        string SourceCode
    );
}