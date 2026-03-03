using VAlgo.Modules.ProblemClassification.Domain.Enums;

namespace VAlgo.Modules.ProblemClassification.Application.Queries.GetActiveClassifications
{
    public sealed class ActiveClassificationDto
    {
        public Guid Id { get; init; }
        public string Code { get; init; } = null!;
        public string Name { get; init; } = null!;
        public ClassificationType Type { get; init; }
    }
}