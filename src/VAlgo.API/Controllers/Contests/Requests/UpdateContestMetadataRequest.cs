namespace VAlgo.API.Controllers.Contests.Requests
{
    public sealed record UpdateContestMetadataRequest(
        string Title,
        string Description
    );
}