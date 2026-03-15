using MediatR;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.SetEditorial
{
    public sealed record SetEditorialCommand(
        Guid ProblemId,
        string Editorial
    ) : IRequest<Unit>;
}