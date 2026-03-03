using MediatR;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemClassification.Application.Commands.DeactivateClassification
{
    public sealed record DeactivateClassificationCommand(
        Guid ClassificationId
    ) : ICommand<Unit>;
}