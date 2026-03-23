using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Domain.Entities
{
    public sealed class ProblemCodeTemplate : Entity<ProblemCodeTemplateId>
    {
        public string Language { get; private set; } = null!;
        public string UserTemplate { get; private set; } = null!;
        public string JudgeTemplate { get; private set; } = null!;

        private ProblemCodeTemplate() { }

        private ProblemCodeTemplate(ProblemCodeTemplateId id, string language, string userTemplate, string judgeTemplate)
            : base(id)
        {
            Language = language;
            UserTemplate = userTemplate;
            JudgeTemplate = judgeTemplate;
        }

        public static ProblemCodeTemplate Create(string language, string userTemplate, string judgeTemplate)
        {
            return new ProblemCodeTemplate(ProblemCodeTemplateId.New(), language, userTemplate, judgeTemplate);
        }

        public void Update(string userTemplate, string judgeTemplate)
        {
            UserTemplate = userTemplate;
            JudgeTemplate = judgeTemplate;
        }
    }
}