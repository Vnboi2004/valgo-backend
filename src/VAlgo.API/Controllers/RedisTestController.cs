using Microsoft.AspNetCore.Mvc;
using VAlgo.SharedKernel.Infrastructure.Redis;

namespace VAlgo.API.Controllers
{
    [ApiController]
    [Route("api/redis-test")]
    public sealed class RedisTestController : Controller
    {
        private readonly RedisDatabaseProvider _redis;

        public RedisTestController(RedisDatabaseProvider redis)
        {
            _redis = redis;
        }

        [HttpGet]
        public async Task<IActionResult> Test()
        {
            var db = _redis.GetDatabase();

            await db.StringSetAsync("test-key", "valgo-redis");

            var value = await db.StringGetAsync("test-key");

            return Ok(value.ToString());
        }
    }
}