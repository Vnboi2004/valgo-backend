namespace VAlgo.API.Controllers.Identity.Requests
{
    public sealed record UpdateUserExperienceRequest(
        string? Work,
        string? Education
    );
}

