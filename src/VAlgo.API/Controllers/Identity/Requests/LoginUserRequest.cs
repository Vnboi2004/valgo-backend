namespace VAlgo.API.Controllers.Identity.Requests
{
    public sealed record LoginUserRequest(
        string Email,
        string Password
    );
}