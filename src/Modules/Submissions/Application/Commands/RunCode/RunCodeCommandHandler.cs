using MediatR;
using VAlgo.Modules.Submissions.Application.Abstractions;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Submissions.Application.Commands.RunCode
{
    public sealed class RunCodeCommandHandler : IRequestHandler<RunCodeCommand, RunCodeResultDto>
    {
        private readonly IRunCodeService _runCodeService;

        public RunCodeCommandHandler(IRunCodeService runCodeService)
        {
            _runCodeService = runCodeService;
        }

        public async Task<RunCodeResultDto> Handle(RunCodeCommand request, CancellationToken cancellationToken)
        {
            return await _runCodeService.RunAsync(request.ProblemId, request.Language, request.SourceCode, cancellationToken);
        }
    }
}