using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using VAlgo.SharedKernel.Messaging;

public sealed class RabbitMqPublisher : IRabbitMqPublisher
{
    private readonly IRabbitMqConnectionProvider _provider;

    public RabbitMqPublisher(IRabbitMqConnectionProvider provider)
    {
        _provider = provider;
    }

    public Task PublishAsync<T>(string queue, T message, CancellationToken ct = default)
    {
        using var channel = _provider.GetConnection().CreateModel();

        channel.QueueDeclare(queue, true, false, false);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        var props = channel.CreateBasicProperties();
        props.Persistent = true;

        channel.BasicPublish("", queue, props, body);

        return Task.CompletedTask;
    }
}
