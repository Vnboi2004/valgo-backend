using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace VAlgo.SharedKernel.Messaging
{
    public sealed class RabbitMqConnectionProvider : IRabbitMqConnectionProvider
    {
        private readonly ConnectionFactory _factory;
        private IConnection? _connection;
        private readonly object _lock = new();

        public RabbitMqConnectionProvider(IConfiguration configuration)
        {
            _factory = new ConnectionFactory
            {
                HostName = configuration["RabbitMQ:Host"],
                Port = int.Parse(configuration["RabbitMQ:Port"] ?? "5672"),
                UserName = configuration["RabbitMQ:Username"],
                Password = configuration["RabbitMQ:Password"]
            };
        }

        public IConnection GetConnection()
        {
            if (_connection != null && _connection.IsOpen)
                return _connection;

            lock (_lock)
            {
                if (_connection == null || !_connection.IsOpen)
                {
                    _connection = _factory.CreateConnection();
                }
            }

            return _connection;
        }
    }
}