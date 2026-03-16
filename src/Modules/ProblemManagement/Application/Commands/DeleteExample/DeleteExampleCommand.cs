using MediatR;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.DeleteExample
{
    public sealed record DeleteExampleCommand(Guid ProblemId, Guid ExampleId) : IRequest<Unit>;
}