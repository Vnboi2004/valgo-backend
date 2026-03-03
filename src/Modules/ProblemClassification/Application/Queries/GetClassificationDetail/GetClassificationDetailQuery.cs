using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemClassification.Application.Queries.GetClassificationDetail
{
    public sealed record GetClassificationDetailQuery(
        Guid ClassificationId
    ) : IQuery<ClassificationDetailDto>;
}