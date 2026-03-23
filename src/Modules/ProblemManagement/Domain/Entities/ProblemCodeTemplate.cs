using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Domain.Entities
{
    public sealed class ProblemCodeTemplate : Entity<ProblemCodeTemplateId>
    {
        public string Language { get; private set; } = null!;
        public string UserTemplate { get; private set; } = null!;
        public string JudgeTemplateHeader { get; private set; } = null!;
        public string JudgeTemplateFooter { get; private set; } = null!;

        private ProblemCodeTemplate() { }

        private ProblemCodeTemplate(ProblemCodeTemplateId id, string language, string userTemplate, string judgeTemplateHeader, string judgeTemplateFooter)
            : base(id)
        {
            Language = language;
            UserTemplate = userTemplate;
            JudgeTemplateHeader = judgeTemplateHeader;
            JudgeTemplateFooter = judgeTemplateFooter;
        }

        public static ProblemCodeTemplate Create(string language, string userTemplate, string judgeTemplateHeader, string judgeTemplateFooter)
        {
            return new ProblemCodeTemplate(ProblemCodeTemplateId.New(), language, userTemplate, judgeTemplateHeader, judgeTemplateFooter);
        }

        public void Update(string userTemplate, string judgeTemplateHeader, string judgeTemplateFooter)
        {
            UserTemplate = userTemplate;
            JudgeTemplateHeader = judgeTemplateHeader;
            JudgeTemplateFooter = judgeTemplateFooter;
        }
    }
}