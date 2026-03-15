using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VAlgo.Modules.ProblemManagement.Domain.Entities;
using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;

namespace VAlgo.Modules.ProblemManagement.Infractructure.Persistence.Configurations
{
    public sealed class SimilarProblemRefConfiguration : IEntityTypeConfiguration<SimilarProblemRef>
    {
        public void Configure(EntityTypeBuilder<SimilarProblemRef> builder)
        {
            builder.ToTable("similar_problems", schema: "problems");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasConversion(x => x.Value, value => SimilarProblemRefId.From(value));

            builder.Property<ProblemId>("problem_id")
                .HasColumnName("problem_id")
                .HasConversion(x => x.Value, value => ProblemId.From(value));

            builder.Property(x => x.ProblemId)
                .HasColumnName("similar_problem_id")
                .HasConversion(x => x.Value, value => ProblemId.From(value))
                .IsRequired();

            builder.HasIndex("problem_id");
        }
    }
}