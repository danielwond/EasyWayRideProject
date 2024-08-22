using EasyWayRide.DataAccess.Data.Entities.configurations;

namespace EasyWayRide.DataAccess.Data.Entities.dbo
{
    public class OnlineDriver
    {
        public Guid ID { get; set; }
        public Driver Driver { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public CarType CarType { get; set; }
        public bool OnRide { get; set; }
    }
}
