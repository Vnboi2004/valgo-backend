using MediatR;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetUserProblemStatus
{
    public sealed record GetUserProblemStatusQuery(Guid ProblemId) : IRequest<UserProblemStatusDto>;
}