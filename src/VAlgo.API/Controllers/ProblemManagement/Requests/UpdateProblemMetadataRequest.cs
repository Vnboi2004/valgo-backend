namespace VAlgo.API.Controllers.ProblemManagement.Requests
{
    public sealed record UpdateProblemMetadataRequest
    (
        string Title,
        string? ShortDescription
    );
}