namespace VAlgo.API.Controllers.Discussions.Requests
{
    public sealed record AddCommentRequest(
        Guid AuthorId,
        string Content,
        Guid? ParentCommentId
    );
}