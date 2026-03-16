using MediatR;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.ReorderExamples
{
    public sealed record ReorderExamplesCommand(
        Guid ProblemId,
        IReadOnlyList<Guid> ExampleIds
    ) : IRequest<Unit>;
}