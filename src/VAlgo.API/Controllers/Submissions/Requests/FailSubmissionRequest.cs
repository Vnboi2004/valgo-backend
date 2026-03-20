
using VAlgo.SharedKernel.CrossModule.Submissions;

namespace VAlgo.API.Controllers.Submissions.Requests
{
    public sealed record FailSubmissionRequest(SubmissionFailureReason Reason);
}