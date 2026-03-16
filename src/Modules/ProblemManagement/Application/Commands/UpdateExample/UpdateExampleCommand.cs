using MediatR;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.UpdateExample
{
    public sealed record UpdateExampleCommand(
        Guid ProblemId,
        Guid ExampleId,
        string Input,
        string Output,
        string? Explanation
    ) : IRequest<Unit>;
}