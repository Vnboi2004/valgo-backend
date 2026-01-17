namespace VAlgo.SharedKernel.Messaging;

public interface IEventHandler<in TEvent>
    where TEvent : IntegrationEvent
{
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken);
}
