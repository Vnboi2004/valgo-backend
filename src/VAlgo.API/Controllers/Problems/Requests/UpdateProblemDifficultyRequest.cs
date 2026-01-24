using VAlgo.Modules.ProblemManagement.Domain.Enums;

namespace VAlgo.API.Controllers.Problems.Requests
{
    public sealed record UpdateProblemDifficultyRequest(Difficulty Difficulty);
}