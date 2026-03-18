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


        public void Rename(string newName)
        {
            if (!IsActive)
                throw new InvalidOperationException("Cannot is active.");

            if (string.IsNullOrWhiteSpace(newName))
                throw new InvalidOperationException("Name is valid.");

            Name = newName.Trim();
        }

        public void Deactivate()
        {
            if (!IsActive)
                return;

            IsActive = false;
        }

        public void Reactivate()
        {
            if (IsActive)
                return;

            IsActive = true;
        }
    }
}