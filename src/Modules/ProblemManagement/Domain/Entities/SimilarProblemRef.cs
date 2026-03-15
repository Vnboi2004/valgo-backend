using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Domain.Entities
{
    public sealed class SimilarProblemRef : Entity<SimilarProblemRefId>
    {
        public ProblemId ProblemId { get; private set; } = null!;

        private SimilarProblemRef() { }

        private SimilarProblemRef(SimilarProblemRefId id, ProblemId problemId) : base(id)
        {
            ProblemId = problemId;
        }

        public static SimilarProblemRef Create(ProblemId problemId)
            => new SimilarProblemRef(SimilarProblemRefId.New(), problemId);
    }
}