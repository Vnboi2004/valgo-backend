namespace VAlgo.API.Controllers.Identity.Requests
{
    public sealed record ResetPasswordUserRequest(string Token, string NewPassword);
}