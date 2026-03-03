using MediatR;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.PublishProblem
{
    public sealed record PublishProblemCommand(Guid ProblemId) : ICommand<Unit>;
}