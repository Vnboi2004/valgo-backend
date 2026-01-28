using MediatR;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemClassification.Application.Commands.RenameClassification
{
    public sealed record RenameClassificationCommand(
        Guid ClassificationId,
        string NewName
    ) : ICommand<Unit>;
}