namespace VAlgo.SharedKernel.Messaging
{
    public interface IRabbitMqPublisher
    {
        Task PublishAsync<T>(string queue, T message, CancellationToken cancellationToken = default);
    }
}