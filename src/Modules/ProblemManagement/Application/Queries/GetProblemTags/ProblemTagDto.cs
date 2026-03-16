
using VAlgo.SharedKernel.CrossModule.Classifications;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemTags
{
    public sealed class ProblemTagDto
    {
        public Guid ClassificationId { get; init; }
        public string Name { get; init; } = null!;
        public ClassificationType Type { get; init; }
    }
}