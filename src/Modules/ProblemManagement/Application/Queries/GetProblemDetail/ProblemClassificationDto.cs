using VAlgo.Modules.ProblemClassification.Domain.Enums;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetCompanyDetail
{
    public sealed class ProblemClassificationDto
    {
        public Guid ClassificationId { get; init; }
        public string Code { get; init; } = null!;
        public string Name { get; init; } = null!;
        public ClassificationType Type { get; init; }
    }
}