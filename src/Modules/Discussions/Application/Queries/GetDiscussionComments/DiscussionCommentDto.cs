namespace VAlgo.Modules.Discussions.Application.Queries.GetDiscussionComments
{
    public sealed class DiscussionCommentDto
    {
        public Guid Id { get; set; }

        public Guid AuthorId { get; set; }

        public string Content { get; set; } = default!;

        public Guid? ParentCommentId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}