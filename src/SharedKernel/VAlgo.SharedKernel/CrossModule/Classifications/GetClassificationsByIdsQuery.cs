using MediatR;

namespace VAlgo.SharedKernel.CrossModule.Classifications
{
    public sealed record GetClassificationsByIdsQuery(IReadOnlyList<Guid> Ids) : IRequest<IReadOnlyList<ClassificationSummaryDto>>;

    public sealed class ClassificationSummaryDto
    {
        public Guid Id { get; init; }
        public string Code { get; init; } = null!;
        public string Name { get; init; } = null!;
        public ClassificationType Type { get; init; }
    }

    public enum ClassificationType
    {
        Tag = 1,
        Category = 2,
        Topic = 3
    }
}