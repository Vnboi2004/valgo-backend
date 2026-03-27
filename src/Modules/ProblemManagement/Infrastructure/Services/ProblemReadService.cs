using VAlgo.Modules.ProblemManagement.Application.Abstractions;
using VAlgo.SharedKernel.CrossModule.Problems;

namespace VAlgo.Modules.ProblemManagement.Infractructure.Services
{
    public sealed class ProblemReadService : IProblemReadService
    {
        private readonly IProblemForJudgeQueries _problemForJudge;

        public ProblemReadService(IProblemForJudgeQueries problemForJudge)
        {
            _problemForJudge = problemForJudge;
        }

        public async Task<ProblemForJudgeDto?> GetForJudgeAsync(Guid problemId, CancellationToken cancellationToken = default)
        {
            var data = await _problemForJudge.GetAsync(problemId, cancellationToken);

            if (data == null)
                return null;

            return new ProblemForJudgeDto
            {
                TimeLimitMs = data.TimeLimitMs,
                MemoryLimitKb = data.MemoryLimitKb,
                SampleTestCases = data.TestCases.Select(x => new TestCaseDto
                {
                    Order = x.Order,
                    Input = x.Input,
                    ExpectedOutput = x.ExpectedOutput
                }).ToList()
            };
        }
    }
}