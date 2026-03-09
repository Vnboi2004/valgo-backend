namespace VAlgo.Modules.Discussions.Application.Queries.GetDiscussionDetail
{
    public sealed class DiscussionDetailDto
    {
        public Guid Id { get; set; }

        public Guid ProblemId { get; set; }

        public Guid AuthorId { get; set; }

        public string Title { get; set; } = default!;

        public string Content { get; set; } = default!;

        public bool IsLocked { get; set; }

        public int CommentCount { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}