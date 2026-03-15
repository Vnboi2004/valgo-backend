using MediatR;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.AddExample
{
    public sealed record AddExampleCommand(
        Guid ProblemId,
        string Input,
        string Output,
        string? Explanation
    ) : IRequest<Unit>;
}