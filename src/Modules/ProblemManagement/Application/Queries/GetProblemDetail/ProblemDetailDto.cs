using VAlgo.Modules.ProblemManagement.Application.Queries.GetCompanyDetail;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemCompanies;
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

        public string? Constraints { get; init; }
        public string? InputFormat { get; init; }
        public string? OutputFormat { get; init; }

        public IReadOnlyList<string> AllowedLanguages { get; init; } = [];
        public IReadOnlyList<ProblemSampleTestCaseDto> Samples { get; init; } = [];
        public IReadOnlyList<ProblemExampleDto> Examples { get; init; } = [];
        public IReadOnlyList<string> Hints { get; init; } = [];
        public IReadOnlyList<ProblemClassificationDto> Classifications { get; init; } = [];
        public IReadOnlyList<ProblemCompanyDto> Companies { get; init; } = [];
    }
}