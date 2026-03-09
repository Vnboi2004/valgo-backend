using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Discussions.Domain.ValueObjects
{
    public sealed class DiscussionId : ValueObject
    {
        public Guid Value { get; }

        private DiscussionId(Guid value) => Value = value;

        public static DiscussionId New() => new(Guid.NewGuid());

        public static DiscussionId From(Guid value) => new(value);

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}