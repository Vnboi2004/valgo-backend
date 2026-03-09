using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VAlgo.Modules.Contests.Domain.Entities;
using VAlgo.Modules.Contests.Domain.ValueObjects;

namespace VAlgo.Modules.Contests.Infrastructure.Persistence.Configurations
{
    public sealed class ContestProblemConfiguration : IEntityTypeConfiguration<ContestProblem>
    {
        public void Configure(EntityTypeBuilder<ContestProblem> builder)
        {
            builder.ToTable("contest_problems");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasConversion(x => x.Value, value => ContestProblemId.From(value));

            builder.Property(x => x.ContestId)
                .HasColumnName("contest_id")
                .HasConversion(x => x.Value, value => ContestId.From(value))
                .IsRequired();

            builder.Property(x => x.ProblemId)
                .HasColumnName("problem_id")
                .IsRequired();

            builder.Property(x => x.Code)
                .HasColumnName("problem_code")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.Order)
                .HasColumnName("order")
                .IsRequired();

            builder.Property(x => x.Points)
                .HasColumnName("points")
                .IsRequired();

            builder.HasIndex(x => x.ContestId);

            builder.HasIndex(x => new { x.ContestId, x.Order })
                .IsUnique();
        }
    }
}