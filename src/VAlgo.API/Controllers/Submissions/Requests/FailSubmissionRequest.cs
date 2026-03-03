using VAlgo.Modules.Submissions.Domain.Enums;

namespace VAlgo.API.Controllers.Submissions.Requests
{
    public sealed record FailSubmissionRequest(SubmissionFailureReason Reason);
}