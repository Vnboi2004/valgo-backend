using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Discussions.Domain.ValueObjects
{
    public sealed class CommentId : ValueObject
    {
        public Guid Value { get; }

        private CommentId(Guid value) => Value = value;

        public static CommentId New() => new(Guid.NewGuid());

        public static CommentId From(Guid value) => new(value);

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}