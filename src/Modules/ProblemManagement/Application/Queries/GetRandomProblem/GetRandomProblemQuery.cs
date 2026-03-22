using MediatR;
using VAlgo.Modules.ProblemManagement.Domain.Enums;
using VAlgo.SharedKernel.CrossModule.Problems;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetRandomProblem
{
    public sealed record GetRandomProblemQuery(Difficulty? Difficulty, Guid? ClassificationId) : IRequest<RandomProblemDto>;
}