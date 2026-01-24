using MediatR;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemClassification.Application.Commands.ReactivateClassification
{
    public sealed record ReactivateClassificationCommand(
        Guid ClassificationId
    ) : ICommand<Unit>;
}