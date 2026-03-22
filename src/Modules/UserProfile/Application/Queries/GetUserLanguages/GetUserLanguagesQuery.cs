using MediatR;
using VAlgo.Modules.UserProfile.Application.DTOs;

namespace VAlgo.Modules.UserProfile.Application.Queries.GetUserLanguages
{
    public sealed record GetUserLanguagesQuery(string Username) : IRequest<IReadOnlyList<UserLanguageStatsDto>>;
}