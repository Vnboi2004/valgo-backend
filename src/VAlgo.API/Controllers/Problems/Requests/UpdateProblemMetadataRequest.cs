namespace VAlgo.API.Controllers.Problems.Requests
{
    public sealed record UpdateProblemMetadataRequest
    (
        string Title,
        string Statement,
        string? ShortDescription
    );
}