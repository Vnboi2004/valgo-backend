using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VAlgo.Modules.Discussions.Domain.Aggregates;
using VAlgo.Modules.Discussions.Domain.ValueObjects;

namespace VAlgo.Modules.Discussions.Infrastructure.Persistence.Configurations
{
    public sealed class DiscussionConfiguration : IEntityTypeConfiguration<Discussion>
    {
        public void Configure(EntityTypeBuilder<Discussion> builder)
        {
            builder.ToTable("discussions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasConversion(x => x.Value, value => DiscussionId.From(value));

            builder.Property(x => x.ProblemId)
                .HasColumnName("problem_id")
                .IsRequired();

            builder.Property(x => x.AuthorId)
                .HasColumnName("author_id")
                .IsRequired();

            builder.Property(x => x.Title)
                .HasColumnName("title")
                .HasMaxLength(200)
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

            builder.Property(x => x.IsLocked)
                .HasColumnName("is_locked")
                .IsRequired();

            builder.Property(x => x.CommentCount)
                .HasColumnName("comment_count")
                .IsRequired();

            builder.HasMany(x => x.Comments)
                .WithOne()
                .HasForeignKey(x => x.DiscussionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.ProblemId);
        }
    }

}