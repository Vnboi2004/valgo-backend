namespace VAlgo.API.Controllers.ProblemManagement.Requests
{
    public sealed record AddCodeTemplateRequest(
        string Language,
        string UserTemplate,
        string JudgeTemplateHeader,
        string JudgeTemplateFooter
    );
}