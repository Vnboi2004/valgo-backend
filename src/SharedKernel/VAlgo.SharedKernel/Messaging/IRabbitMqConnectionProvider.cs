using RabbitMQ.Client;

namespace VAlgo.SharedKernel.Messaging
{
    public interface IRabbitMqConnectionProvider
    {
        IConnection GetConnection();
    }
}