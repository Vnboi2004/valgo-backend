using MediatR;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Submissions.Application.Commands.CancelSubmission
{
    public sealed record CancelSubmissionCommand(Guid SubmissionId, string? Reason = null) : ICommand<Unit>;
}