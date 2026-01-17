namespace VAlgo.SharedKernel.Abstractions;

public abstract class Entity<TId>
{
    public TId Id { get; protected set; } = default!;

    protected Entity() { }

    protected Entity(TId id)
    {
        Id = id;
    }

    public override bool Equals(object? obj)
        => obj is Entity<TId> other && EqualityComparer<TId>.Default.Equals(Id, other.Id);

    public override int GetHashCode() => Id!.GetHashCode();
}