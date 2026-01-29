namespace VAlgo.API.Controllers.Identity.Requests
{
    public sealed record ChangePasswordUserRequest(
        Guid UserId,
        string CurrentPassword,
        string NewPassword
    );
}