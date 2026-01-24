using MediatR;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.AssignClassification
{
    public sealed record AssignClassificationCommand(
        Guid ProblemId,
        Guid ClassificationId
    ) : ICommand<Unit>;
}