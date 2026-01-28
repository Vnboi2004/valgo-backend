using MediatR;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.UnassignClassification
{
    public sealed record UnassignClassificationCommand(Guid ProblemId, Guid ClassificationId) : ICommand<Unit>;
}