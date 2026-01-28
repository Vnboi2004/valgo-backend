using VAlgo.Modules.ProblemManagement.Domain.Enums;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemDetail
{
    public sealed class ProblemDetailDto
    {
        public Guid ProblemId { get; init; }
        public string Code { get; init; } = null!;
        public string Title { get; init; } = null!;
        public string Statement { get; init; } = null!;
        public Difficulty Difficulty { get; init; }

        public int TimeLimitMs { get; init; }
        public int MemoryLimitKb { get; init; }

        public IReadOnlyList<string> AllowedLanguages { get; init; } = [];
        public IReadOnlyList<ProblemSampleTestCaseDto> Samples { get; init; } = [];
        public IReadOnlyList<Guid> ClassificationIds { get; init; } = [];
    }
}