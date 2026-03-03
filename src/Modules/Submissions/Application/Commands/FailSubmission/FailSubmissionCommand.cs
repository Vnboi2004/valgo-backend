using MediatR;
using VAlgo.Modules.Submissions.Domain.Enums;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Submissions.Application.Commands.FailSubmission
{
    public sealed record FailSubmissionCommand(Guid SubmissionId, SubmissionFailureReason Reason) : ICommand<Unit>;
}