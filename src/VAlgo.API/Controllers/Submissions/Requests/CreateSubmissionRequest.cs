namespace VAlgo.API.Controllers.Submissions.Requests
{
    public sealed record CreateSubmissionRequest(
        Guid UserId,
        Guid ProblemId,
        string Language,
        string SourceCode
    );
}