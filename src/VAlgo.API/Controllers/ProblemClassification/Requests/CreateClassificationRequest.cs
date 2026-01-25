using VAlgo.Modules.ProblemClassification.Domain.Enums;

namespace VAlgo.API.Controllers.ProblemClassification.Requests
{
    public sealed record CreateClassificationRequest(
        string Code,
        string Name,
        ClassificationType Type
    );
}