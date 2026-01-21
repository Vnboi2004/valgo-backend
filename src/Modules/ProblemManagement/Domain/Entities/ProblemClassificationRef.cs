using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Domain.Entities
{
    public sealed class ProblemClassificationRef : Entity<ProblemClassificationRefId>
    {
        public Guid ClassificationId { get; private set; }

        internal ProblemClassificationRef(ProblemClassificationRefId id, Guid classificationId) : base(id)
        {
            ClassificationId = classificationId;
        }

        internal static ProblemClassificationRef Create(Guid classificationId)
            => new ProblemClassificationRef(ProblemClassificationRefId.New(), classificationId);
    }
}