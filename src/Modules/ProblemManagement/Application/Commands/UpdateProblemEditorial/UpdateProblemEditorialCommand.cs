using MediatR;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.UpdateProblemEditorial
{
    public sealed record UpdateProblemEditorialCommand(
        Guid ProblemId,
        string Editorial
    ) : IRequest<Unit>;
}