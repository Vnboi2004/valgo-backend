
using VAlgo.SharedKernel.CrossModule.Classifications;

namespace VAlgo.API.Controllers.ProblemClassification.Requests
{
    public sealed record CreateClassificationRequest(
        string Code,
        string Name,
        ClassificationType Type
    );
}