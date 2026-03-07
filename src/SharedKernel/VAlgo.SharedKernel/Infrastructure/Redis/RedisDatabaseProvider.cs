using StackExchange.Redis;

namespace VAlgo.SharedKernel.Infrastructure.Redis
{
    public sealed class RedisDatabaseProvider
    {
        private readonly RedisConnectionStringFactory _factory;

        public RedisDatabaseProvider(RedisConnectionStringFactory factory)
        {
            _factory = factory;
        }

        public IDatabase GetDatabase()
        {
            return _factory.Connection.GetDatabase();
        }
    }
}