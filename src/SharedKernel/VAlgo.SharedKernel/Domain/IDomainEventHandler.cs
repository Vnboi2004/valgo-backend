using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.SharedKernel.Domain
{
    public interface IDomainEventHandler<in TEvent>
    where TEvent : IDomainEvent
    {
        Task Handle(TEvent domainEvent, CancellationToken cancellationToken);
    }

}