using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VAlgo.Modules.Contests.Domain.Entities;
using VAlgo.Modules.Contests.Domain.ValueObjects;

namespace VAlgo.Modules.Contests.Infrastructure.Persistence.Configurations
{
    public sealed class VirtualContestSessionConfiguration : IEntityTypeConfiguration<VirtualContestSession>
    {
        public void Configure(EntityTypeBuilder<VirtualContestSession> builder)
        {
            builder.ToTable("virtual_contest_sessions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasConversion(id => id.Value, value => VirtualContestSessionId.From(value));

            builder.Property(x => x.ContestId)
                .HasColumnName("contest_id")
                .HasConversion(id => id.Value, value => ContestId.From(value))
                .IsRequired();

            builder.Property(x => x.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(x => x.StartTime)
                .HasColumnName("start_time")
                .IsRequired();

            builder.Property(x => x.EndTime)
                .HasColumnName("end_time")
                .IsRequired();

            builder.Property(x => x.Score)
                .HasColumnName("score")
                .IsRequired();

            builder.Property(x => x.Penalty)
                .HasColumnName("penalty")
                .IsRequired();

            builder.HasMany(x => x.ProblemStats)
                .WithOne()
                .HasForeignKey(x => x.VirtualContestSessionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.ContestId);

            builder.HasIndex(x => new { x.ContestId, x.UserId })
                .IsUnique();
        }
    }
}