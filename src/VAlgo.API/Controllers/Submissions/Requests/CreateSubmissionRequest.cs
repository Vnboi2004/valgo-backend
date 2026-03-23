namespace VAlgo.API.Controllers.Submissions.Requests
{
    public sealed record CreateSubmissionRequest(
        Guid ProblemId,
        Guid? ContestId,
        string Language,
        string SourceCode
    );
}