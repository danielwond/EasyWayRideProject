using StackExchange.Redis;

namespace EasyWayRide.Services.Services.CachingService
{
    public class CachingService : ICachingService
    {
        public ConnectionMultiplexer InitializeRedis()
        {
            ConfigurationOptions options = new()
            {
                EndPoints = { { "localhost", 6379 } },
                Password = "secret", // use your Redis password
            };

            ConnectionMultiplexer rediss = ConnectionMultiplexer.Connect(options);
            return rediss;
        }
    }
}
