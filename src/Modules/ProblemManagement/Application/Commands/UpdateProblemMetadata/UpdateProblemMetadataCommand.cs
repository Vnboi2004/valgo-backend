using MediatR;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.UpdateProblemMetadata
{
    public sealed record UpdateProblemMetadataCommand(
        Guid ProblemId,
        string Title,
        string? ShortDescription
    ) : ICommand<Unit>;
}