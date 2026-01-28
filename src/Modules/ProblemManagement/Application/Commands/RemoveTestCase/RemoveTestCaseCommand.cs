using MediatR;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.RemoveTestCase
{
    public sealed record RemoveTestCaseCommand(Guid ProblemId, Guid TestCaseId) : ICommand<Unit>;
}