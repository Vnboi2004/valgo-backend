namespace VAlgo.API.Controllers.Identity.Requests
{
    public sealed record RegisterUserRequest(
        string Username,
        string Email,
        string Password
    );
}