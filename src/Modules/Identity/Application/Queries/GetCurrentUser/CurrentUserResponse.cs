namespace VAlgo.Modules.Identity.Application.Queries.GetCurrentUser
{
    public sealed record CurrentUserResponse(
        Guid Id,
        string Email,
        string UserName,
        string Role,
        bool EmailVerfied
    );
}