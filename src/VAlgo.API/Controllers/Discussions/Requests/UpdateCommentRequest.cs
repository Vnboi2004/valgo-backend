namespace VAlgo.API.Controllers.Discussions.Requests
{
    public sealed record UpdateCommentRequest(
        Guid UserId,
        string Content
    );
}