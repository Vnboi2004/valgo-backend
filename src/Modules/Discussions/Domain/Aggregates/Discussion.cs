using VAlgo.Modules.Discussions.Domain.Entities;
using VAlgo.Modules.Discussions.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Discussions.Domain.Aggregates
{
    public sealed class Discussion : AggregateRoot<DiscussionId>
    {
        public Guid ProblemId { get; private set; }
        public Guid AuthorId { get; private set; }
        public string Title { get; private set; } = null!;
        public string Content { get; private set; } = null!;
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public bool IsLocked { get; private set; }
        public int CommentCount { get; private set; }
        private readonly List<Comment> _comments = new();
        public IReadOnlyCollection<Comment> Comments => _comments;

        private Discussion() { }

        private Discussion(DiscussionId id, Guid problemId, Guid authorId, string title, string content)
            : base(id)
        {
            ProblemId = problemId;
            AuthorId = authorId;
            Title = title;
            Content = content;
            CreatedAt = DateTime.UtcNow;
            IsLocked = false;
        }

        public static Discussion Create(Guid problemId, Guid authorId, string title, string content)
        {
            return new Discussion(DiscussionId.New(), problemId, authorId, title, content);
        }

        public void UpdateContent(string title, string content)
        {
            Title = title;
            Content = content;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Lock()
        {
            IsLocked = true;
        }

        public void UnLock()
        {
            IsLocked = false;
        }

        public Comment AddComment(Guid authorId, string content, CommentId? parentCommentId)
        {
            if (IsLocked)
                throw new InvalidOperationException("Dicussion is locked.");

            var comment = Comment.Create(Id, authorId, content, parentCommentId);

            _comments.Add(comment);

            CommentCount++;

            return comment;
        }
    }
}