using StackExchange.Redis;

namespace EasyWayRide.Services.Services.CachingService
{
    public interface ICachingService
    {
        public ConnectionMultiplexer InitializeRedis();
    }
}
