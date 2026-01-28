using MediatR;

namespace VAlgo.Modules.Submissions.Application.Commands.EnqueueSubmission
{
    public sealed record EnqueueSubmissionCommand(Guid SubmissionId) : IRequest<Unit>;
}