using MediatR;

namespace VAlgo.SharedKernel.Abstractions;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}
