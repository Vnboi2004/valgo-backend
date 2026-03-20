using MediatR;
using VAlgo.SharedKernel.Abstractions;
using VAlgo.SharedKernel.CrossModule.Submissions;

namespace VAlgo.Modules.Submissions.Application.Commands.FailSubmission
{
    public sealed record FailSubmissionCommand(Guid SubmissionId, SubmissionFailureReason Reason) : ICommand<Unit>;
}