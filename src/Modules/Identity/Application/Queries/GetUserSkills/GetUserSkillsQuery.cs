using MediatR;

namespace VAlgo.Modules.Identity.Application.Queries.GetUserSkills
{
    public sealed record GetUserSkillsQuery(string Username) : IRequest<UserSkillsDto>;
}