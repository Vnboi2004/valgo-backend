using VAlgo.Modules.ProblemManagement.Domain.Enums;

namespace VAlgo.API.Controllers.ProblemManagement.Requests
{
    public sealed record UpdateProblemDifficultyRequest(Difficulty Difficulty);
}