namespace VAlgo.Modules.Identity.Application.Commands.LoginUser
{
    public sealed record LoginResult(
        Guid UserId,
        string AccessToken,
        string RefreshToken
    );
}