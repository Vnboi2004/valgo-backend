using MediatR;

namespace VAlgo.Modules.Identity.Application.Queries.GetUserLanguages
{
    public sealed record GetUserLanguagesQuery(string Username) : IRequest<IReadOnlyList<UserLanguageStatsDto>>;
}