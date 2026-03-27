using VAlgo.BuildingBlocks.Sandbox.Abstractions;
using VAlgo.BuildingBlocks.Sandbox.Judging;
using VAlgo.BuildingBlocks.Sandbox.Models;
using VAlgo.Modules.Submissions.Application.Abstractions;
using VAlgo.Modules.Submissions.Application.Commands.RunCode;
using VAlgo.SharedKernel.CrossModule.Problems;
using VAlgo.SharedKernel.CrossModule.Submissions;

namespace VAlgo.Modules.Submissions.Infrastructure.Services
{
    public sealed class RunCodeService : IRunCodeService
    {
        private readonly ISandboxRunner _sandbox;
        private readonly IProblemReadService _problemReadService;
        private readonly ICodeTemplateReadService _codeTemplateReadService;

        public RunCodeService(ISandboxRunner sandbox, IProblemReadService problemReadService, ICodeTemplateReadService codeTemplateReadService)
        {
            _sandbox = sandbox;
            _problemReadService = problemReadService;
            _codeTemplateReadService = codeTemplateReadService;
        }

        public async Task<RunCodeResultDto> RunAsync(Guid problemId, string language, string sourceCode, CancellationToken cancellationToken)
        {
            var problem = await _problemReadService.GetForJudgeAsync(problemId, cancellationToken);
            if (problem == null)
                throw new Exception("Problem not found.");

            var template = await _codeTemplateReadService.GetTemplateAsync(problemId, language, cancellationToken);
            if (template == null)
                throw new Exception("Code template not found.");

            var fullCode = template.GetFullCode(sourceCode);

            var lang = SandboxLanguageRegistry.Resolve(language);

            var compile = await _sandbox.CompileAsync(new SandboxCompileRequest(fullCode, lang), cancellationToken);

            if (!compile.Success)
            {
                return new RunCodeResultDto
                {
                    Verdict = Verdict.CompileError.ToString()
                };
            }

            var workDir = compile.WorkDir;

            int maxTime = 0;
            int maxMemory = 0;
            Verdict finalVerdict = Verdict.Accepted;

            var result = new List<RunCodeTestCaseDto>();

            foreach (var tc in problem.SampleTestCases.OrderBy(x => x.Order))
            {
                var run = await _sandbox.RunAsync(
                    new SandboxRunRequest(fullCode, tc.Input, lang, problem.TimeLimitMs, problem.MemoryLimitKb),
                    workDir,
                    cancellationToken
                );

                maxTime = Math.Max(maxTime, run.TimeMs);
                maxMemory = Math.Max(maxMemory, run.MemoryKb);

                Verdict caseVerdict;

                if (run.Verdict != Verdict.Accepted)
                {
                    caseVerdict = run.Verdict;
                    finalVerdict = run.Verdict;
                }
                else if (!OutputComparer.Equals(run.Stdout, tc.ExpectedOutput))
                {
                    caseVerdict = Verdict.WrongAnswer;
                    finalVerdict = Verdict.WrongAnswer;
                }
                else
                {
                    caseVerdict = Verdict.Accepted;
                }


                result.Add(new RunCodeTestCaseDto
                {
                    Index = tc.Order,
                    Passed = caseVerdict == Verdict.Accepted,
                    Output = run.Stdout,
                    Expected = tc.ExpectedOutput,
                    TimeMs = run.TimeMs,
                    MemoryKb = run.MemoryKb,
                    Verdict = caseVerdict.ToString()
                });
            }

            return new RunCodeResultDto
            {
                Verdict = finalVerdict.ToString(),
                MaxTimeMs = maxTime,
                MaxMemoryKb = maxMemory,
                TestCases = result
            };
        }
    }
}