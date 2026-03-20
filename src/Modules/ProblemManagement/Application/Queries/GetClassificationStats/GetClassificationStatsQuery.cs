using MediatR;
using VAlgo.SharedKernel.CrossModule.Classifications;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetClassificationStats
{
    public sealed record GetClassificationStatsQuery(ClassificationType? Type) : IRequest<IReadOnlyList<ClassificationStatsDto>>;
}