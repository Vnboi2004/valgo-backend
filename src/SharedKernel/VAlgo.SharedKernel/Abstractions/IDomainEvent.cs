namespace VAlgo.SharedKernel.Abstractions;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
