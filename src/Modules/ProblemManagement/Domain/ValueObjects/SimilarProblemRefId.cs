using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Domain.ValueObjects
{
    public sealed class SimilarProblemRefId : ValueObject
    {
        public Guid Value { get; }

        private SimilarProblemRefId(Guid value) => Value = value;

        public static SimilarProblemRefId New() => new(Guid.NewGuid());

        public static SimilarProblemRefId From(Guid value) => new(value);

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}