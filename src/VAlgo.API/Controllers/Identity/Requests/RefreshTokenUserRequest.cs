namespace VAlgo.API.Controllers.Identity.Requests
{
    public sealed record RefreshTokenUserRequest(
        string RefreshToken
    );
}