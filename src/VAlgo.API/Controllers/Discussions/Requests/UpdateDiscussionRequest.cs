namespace VAlgo.API.Controllers.Discussions.Requests
{
    public sealed record UpdateDiscussionRequest(
        Guid UserId,
        string Title,
        string Content
    );
}