using MediatR;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemEditorial
{
    public sealed record GetProblemEditorialQuery(Guid ProblemId) : IRequest<ProblemEditorialDto>;
}