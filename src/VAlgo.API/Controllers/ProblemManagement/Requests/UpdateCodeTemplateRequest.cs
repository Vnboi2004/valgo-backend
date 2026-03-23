namespace VAlgo.API.Controllers.ProblemManagement.Requests
{
    public sealed record UpdateCodeTemplateRequest(
        string UserTemplate,
        string JudgeTemplateHeader,
        string JudgeTemplateFooter
    );
}