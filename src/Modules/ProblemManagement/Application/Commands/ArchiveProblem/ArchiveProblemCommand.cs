using MediatR;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.ArchiveProblem
{
    public sealed record ArchiveProblemCommand(Guid ProblemId) : ICommand<Unit>;
}                                   