using VAlgo.Modules.ProblemClassification.Domain.Enums;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemClassification.Application.Commands.CreateClassification
{
    public sealed record CreateClassificationCommand(
        string Code,
        string Name,
        ClassificationType Type
    ) : ICommand<Guid>;
}