using VAlgo.Modules.Discussions.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Discussions.Domain.Entities
{
    public sealed class Comment : Entity<CommentId>
    {
        public DiscussionId DiscussionId { get; private set; } = null!;
        public Guid AuthorId { get; private set; }
        public string Content { get; private set; } = null!;
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public CommentId? ParentCommentId { get; private set; }

        private Comment() { }

        private Comment(CommentId id, DiscussionId discussionId, Guid authorId, string content, CommentId? parentCommentId)
            : base(id)
        {
            DiscussionId = discussionId;
            AuthorId = authorId;
            Content = content;
            ParentCommentId = parentCommentId;
            CreatedAt = DateTime.UtcNow;
        }

        public static Comment Create(DiscussionId discussionId, Guid authorId, string content, CommentId? parentCommentId)
        {
            return new Comment(CommentId.New(), discussionId, authorId, content, parentCommentId);
        }

        public void UpdateContent(string content)
        {
            Content = content;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}