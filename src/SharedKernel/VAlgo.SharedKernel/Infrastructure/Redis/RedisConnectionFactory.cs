using StackExchange.Redis;

namespace VAlgo.SharedKernel.Infrastructure.Redis
{
    public sealed class RedisConnectionStringFactory
    {
        private readonly Lazy<ConnectionMultiplexer> _connection;

        public RedisConnectionStringFactory(string connectionString)
        {
            _connection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(connectionString));
        }

        public ConnectionMultiplexer Connection => _connection.Value;
    }
}