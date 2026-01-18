using MediatR;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Submissions.Application.Commands.FailSubmission
{
    public sealed record FailSubmissionCommand(Guid SubmissionId, string Reason) : ICommand<Unit>;
}