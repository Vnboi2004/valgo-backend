using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Domain.Aggregates
{
    public sealed class Company : AggregateRoot<CompanyId>
    {
        public string Name { get; private set; } = null!;
        public bool IsActive { get; private set; }

        private Company() { }

        private Company(CompanyId id, string name) : base(id)
        {
            Name = name;
            IsActive = true;
        }

        public static Company Create(string name)
            => new Company(CompanyId.New(), name);
    }
}