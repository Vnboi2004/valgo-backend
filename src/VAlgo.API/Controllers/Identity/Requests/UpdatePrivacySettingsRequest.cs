namespace VAlgo.API.Controllers.Identity.Requests
{
    public sealed record UpdatePrivacySettingsRequest(
        bool ShowRecentSubmissions,
        bool ShowSubmissionHeatmap
    );
}