using MediatR;
using VAlgo.Modules.ProblemManagement.Domain.Enums;
using VAlgo.SharedKernel.Abstractions;
using VAlgo.SharedKernel.CrossModule.Problems;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.UpdateProblemDifficulty
{
    public sealed record UpdateProblemDifficultyCommand(Guid ProblemId, Difficulty Difficulty) : ICommand<Unit>;
}