using VAlgo.Modules.ProblemManagement.Domain.Enums;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemEditor
{
    public sealed class ProblemEditorDto
    {
        public Guid ProblemId { get; init; }

        public string Code { get; init; } = null!;
        public string Title { get; init; } = null!;
        public string Statement { get; init; } = null!;
        public string? ShortDescription { get; init; }

        public string? Constraints { get; init; }
        public string? InputFormat { get; init; }
        public string? OutputFormat { get; init; }
        public string? FollowUp { get; init; }
        public string? Editorial { get; init; }

        public Difficulty Difficulty { get; init; }
        public ProblemStatus Status { get; init; }

        public int TimeLimitMs { get; init; }
        public int MemoryLimitKb { get; init; }

        public IReadOnlyList<string> AllowedLanguages { get; init; } = [];

        public IReadOnlyList<Guid> ClassificationIds { get; init; } = [];

        public IReadOnlyList<Guid> CompanyIds { get; init; } = [];

        public IReadOnlyList<Guid> SimilarProblemIds { get; init; } = [];

        public IReadOnlyList<ProblemExampleEditorDto> Examples { get; init; } = [];

        public IReadOnlyList<ProblemHintEditorDto> Hints { get; init; } = [];

        public IReadOnlyList<ProblemEditorTestCaseDto> TestCases { get; init; } = [];
    }
}