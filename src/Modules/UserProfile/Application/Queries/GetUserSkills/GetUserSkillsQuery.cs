using MediatR;
using VAlgo.Modules.UserProfile.Application.DTOs;

namespace VAlgo.Modules.UserProfile.Application.Queries.GetUserSkills
{
    public sealed record GetUserSkillsQuery(string Username) : IRequest<UserSkillsDto>;
}