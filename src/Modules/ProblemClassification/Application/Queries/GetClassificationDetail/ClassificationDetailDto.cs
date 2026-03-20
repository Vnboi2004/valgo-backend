
using VAlgo.SharedKernel.CrossModule.Classifications;

namespace VAlgo.Modules.ProblemClassification.Application.Queries.GetClassificationDetail
{
    public sealed class ClassificationDetailDto
    {
        public Guid Id { get; init; }
        public string Code { get; init; } = null!;
        public string Name { get; init; } = null!;
        public ClassificationType Type { get; init; }
        public bool IsActive { get; init; }
    }
}