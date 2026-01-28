using MediatR;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.UpdateProblemMetadata
{
    public sealed record UpdateProblemMetadataCommand(
        Guid ProblemId,
        string Title,
        string Statement,
        string? ShortDescription
    ) : ICommand<Unit>;
}