using VAlgo.Modules.ProblemClassification.Domain.Enums;
using VAlgo.Modules.ProblemClassification.Domain.Exceptions;
using VAlgo.Modules.ProblemClassification.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemClassification.Domain.Aggregates
{
    public sealed class Classification : AggregateRoot<ClassificationId>
    {
        public string Code { get; private set; } = null!;
        public string Name { get; private set; } = null!;
        public ClassificationType Type { get; private set; }
        public bool IsActive { get; private set; }

        private Classification() { }

        private Classification(
            ClassificationId id,
            string code,
            string name,
            ClassificationType type
        ) : base(id)
        {
            Code = NormalizeCode(code);
            Name = name;
            Type = type;
            IsActive = true;
        }

        public static Classification Create(string code, string name, ClassificationType type)
            => new Classification(ClassificationId.New(), code, name, type);

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

        public void Rename(string newName)
        {
            if (!IsActive)
                throw new ClassificationInactiveException();

            if (string.IsNullOrWhiteSpace(newName))
                throw new ClassificationNameInvalidException();

            Name = newName.Trim();
        }

        private static string NormalizeCode(string code)
            => code.Trim().ToUpperInvariant();
    }
}