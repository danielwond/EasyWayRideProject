using StackExchange.Redis;

namespace EasyWayRide.DataAccess.Repos.ICachingRepo
{
    public class CachingRepository : ICachingRepository
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
