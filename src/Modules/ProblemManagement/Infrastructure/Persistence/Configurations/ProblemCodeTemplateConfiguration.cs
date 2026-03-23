using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VAlgo.Modules.ProblemManagement.Domain.Entities;
using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;

namespace VAlgo.Modules.ProblemManagement.Infractructure.Persistence.Configurations
{
    public sealed class ProblemCodeTemplateConfiguration : IEntityTypeConfiguration<ProblemCodeTemplate>
    {
        public void Configure(EntityTypeBuilder<ProblemCodeTemplate> builder)
        {
            builder.ToTable("problem_code_template", schema: "problems");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasConversion(x => x.Value, value => ProblemCodeTemplateId.From(value))
                .ValueGeneratedNever();

            builder.Property(x => x.Language)
                .HasColumnName("language")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.UserTemplate)
                .HasColumnName("user_template")
                .IsRequired();

            builder.Property(x => x.JudgeTemplateHeader)
                .HasColumnName("judge_template_header")
                .IsRequired();

            builder.Property(x => x.JudgeTemplateFooter)
                .HasColumnName("judge_template_footer")
                .IsRequired();
        }
    }
}