using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Domain.Entities
{
    public sealed class ProblemCompanyRef : Entity<ProblemCompanyRefId>
    {
        public Guid CompanyId { get; private set; }

        private ProblemCompanyRef() { }

        private ProblemCompanyRef(ProblemCompanyRefId id, Guid companyId) : base(id)
        {
            CompanyId = companyId;
        }

        public static ProblemCompanyRef Create(Guid companyId)
            => new ProblemCompanyRef(ProblemCompanyRefId.New(), companyId);
    }
}