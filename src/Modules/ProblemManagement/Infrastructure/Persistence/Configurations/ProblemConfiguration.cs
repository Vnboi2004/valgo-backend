using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VAlgo.Modules.ProblemManagement.Domain.Aggregates;
using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;

namespace VAlgo.Modules.ProblemManagement.Infractructure.Persistence.Configurations
{
    public sealed class ProblemConfiguration : IEntityTypeConfiguration<Problem>
    {
        public void Configure(EntityTypeBuilder<Problem> builder)
        {
            builder.ToTable("problems", schema: "problems");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasConversion(id => id.Value, value => ProblemId.From(value))
                .ValueGeneratedNever();

            builder.Property(x => x.Code)
                .HasColumnName("code")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Title)
                .HasColumnName("title")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Statement)
                .HasColumnName("statement")
                .IsRequired();

            builder.Property(x => x.ShortDescription)
                .HasColumnName("short_description")
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(x => x.Constraints)
                .HasColumnName("constraints")
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(x => x.InputFormat)
                .HasColumnName("input_format")
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(x => x.OutputFormat)
                .HasColumnName("output_format")
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(x => x.FollowUp)
                .HasColumnName("follow_up")
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(x => x.Editorial)
                .HasColumnName("editorial")
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(x => x.Difficulty)
                .HasColumnName("difficulty")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(x => x.Status)
                .HasColumnName("status")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(x => x.TimeLimitMs)
                .HasColumnName("time_limit_ms")
                .IsRequired();

            builder.Property(x => x.MemoryLimitKb)
                .HasColumnName("memory_limit_kb")
                .IsRequired();

            builder.Navigation(x => x.TestCases)
                .AutoInclude(false);

            builder.Navigation(x => x.Classifications)
                .AutoInclude(false);

            builder.OwnsMany(x => x.AllowedLanguages, b =>
            {
                b.ToTable("problem_allowed_languages", "problems");

                b.WithOwner()
                    .HasForeignKey("ProblemId");

                b.Property<ProblemId>("ProblemId")
                    .HasColumnName("problem_id")
                    .HasConversion(
                        id => id.Value,
                        value => ProblemId.From(value)
                    );

                b.Property(x => x.Value)
                    .HasColumnName("language")
                    .HasMaxLength(50)
                    .IsRequired();

                b.HasKey("ProblemId", nameof(AllowedLanguage.Value));
            });

            builder.HasMany(x => x.Examples)
                .WithOne()
                .HasForeignKey("problem_id")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Hints)
                .WithOne()
                .HasForeignKey("problem_id")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Companies)
                .WithOne()
                .HasForeignKey("problem_id")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.SimilarProblems)
                .WithOne()
                .HasForeignKey("problem_id")
                .OnDelete(DeleteBehavior.Cascade);


            builder.Metadata
                .FindNavigation(nameof(Problem.AllowedLanguages))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}