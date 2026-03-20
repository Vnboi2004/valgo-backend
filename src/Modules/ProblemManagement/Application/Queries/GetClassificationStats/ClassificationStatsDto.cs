
using VAlgo.SharedKernel.CrossModule.Classifications;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetClassificationStats
{
    // ClassificationStatsDto
    public sealed class ClassificationStatsDto
    {
        public Guid ClassificationId { get; init; }
        public string Code { get; init; } = null!;
        public string Name { get; init; } = null!;
        public ClassificationType Type { get; init; }
        public int ProblemCount { get; init; }
    }
}