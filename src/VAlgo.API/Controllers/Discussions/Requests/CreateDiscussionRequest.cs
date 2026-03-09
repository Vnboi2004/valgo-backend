namespace VAlgo.API.Controllers.Discussions.Requests
{
    public sealed record CreateDiscussionRequest(
        Guid AuthorId,
        string Title,
        string Content
    );
}