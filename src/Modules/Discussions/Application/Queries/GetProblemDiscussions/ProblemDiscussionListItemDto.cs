namespace VAlgo.Modules.Discussions.Application.Queries.GetProblemDiscussions
{
    public sealed class ProblemDiscussionListItemDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = default!;

        public Guid AuthorId { get; set; }

        public bool IsLocked { get; set; }

        public int CommentCount { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}