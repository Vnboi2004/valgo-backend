using MediatR;

namespace VAlgo.Modules.Identity.Application.Commands.UpdatePrivacySettings
{
    public sealed record UpdatePrivacySettingsCommand(
        bool ShowRecentSubmissions,
        bool ShowSubmissionHeatmap
    ) : IRequest<Unit>;
}