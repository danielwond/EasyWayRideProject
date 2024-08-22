using StackExchange.Redis;

namespace EasyWayRide.DataAccess.Repos.ICachingRepo
{
    public interface ICachingRepository
    {
        public ConnectionMultiplexer InitializeRedis();
    }
}
