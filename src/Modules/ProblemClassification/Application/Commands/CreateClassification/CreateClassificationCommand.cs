using VAlgo.SharedKernel.Abstractions;
using VAlgo.SharedKernel.CrossModule.Classifications;

namespace VAlgo.Modules.ProblemClassification.Application.Commands.CreateClassification
{
    public sealed record CreateClassificationCommand(
        string Code,
        string Name,
        ClassificationType Type
    ) : ICommand<Guid>;
}