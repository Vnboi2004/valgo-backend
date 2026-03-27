using VAlgo.Modules.Submissions.Application.Commands.RunCode;

namespace VAlgo.Modules.Submissions.Application.Abstractions
{
    public interface IRunCodeService
    {
        Task<RunCodeResultDto> RunAsync(Guid problemId, string language, string sourceCode, CancellationToken cancellationToken);
    }
}