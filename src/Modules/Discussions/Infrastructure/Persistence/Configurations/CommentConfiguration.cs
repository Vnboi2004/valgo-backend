using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VAlgo.Modules.Discussions.Domain.Entities;
using VAlgo.Modules.Discussions.Domain.ValueObjects;

namespace VAlgo.Modules.Discussions.Infrastructure.Persistence.Configurations
{
    public sealed class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("comments");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasConversion(x => x.Value, value => CommentId.From(value));

            builder.Property(x => x.DiscussionId)
                .HasColumnName("discussion_id")
                .HasConversion(x => x.Value, value => DiscussionId.From(value))
                .IsRequired();

            builder.Property(x => x.AuthorId)
                .HasColumnName("author_id")
                .IsRequired();

            builder.Property(x => x.Content)
                .HasColumnName("content")
                .HasMaxLength(2000)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired(false);

            builder.Property(x => x.ParentCommentId)
                .HasColumnName("parent_comment_id")
                .HasConversion(
                    (CommentId? x) => x == null ? null : x.Value,
                    (Guid? value) => value == null ? null : CommentId.From(value.Value))
                .IsRequired(false);

            builder.HasIndex(x => x.DiscussionId);
        }
    }
}