using MediatR;

namespace VAlgo.Modules.Submissions.Application.Commands.RunCode
{
    public sealed record RunCodeCommand(
        Guid ProblemId,
        string Language,
        string SourceCode
    ) : IRequest<RunCodeResultDto>;
}