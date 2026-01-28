using MediatR;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Submissions.Application.Commands.StartSubmissionExecution
{
    public sealed record StartSubmissionExecutionCommand(Guid SubmissionId) : ICommand<Unit>;
}